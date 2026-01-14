using System.Security.Claims;
using AuthService.DTOs.Auth;
using AuthService.DTOs.User;
using AuthService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Security.Interfaces;

namespace AuthService.Controllers
{
   // NOTE:
   // Identity is created separatly and do not use Mediator
   // It is outside of Application project where the Business Logic is
   [ApiController]
   [Route("api/[controller]")]
   // [Route("api/v{version:apiVersion}/[controller]")]
   // [ApiVersion("1.0")]
   // [ApiVersion("2.0")]
   public class AccountController : ControllerBase
   {
      private readonly IUserTokenService<AppUser> _tokenService;
      private readonly UserManager<AppUser> _userManager;
      private readonly RoleManager<AppRole> _roleManager;
      private readonly IMapper _mapper;
      public AccountController(
         IUserTokenService<AppUser> tokenService,
         UserManager<AppUser> userManager,
         RoleManager<AppRole> roleManager,
         IMapper mapper)
      {
         _tokenService = tokenService;
         _userManager = userManager;
         _roleManager = roleManager;
         _mapper = mapper;
      }

      [AllowAnonymous]
      [HttpPost("login")]
      public async Task<IActionResult> Login(LoginDto loginDto)
      {
         // => If UserName is provided check for UserName
         var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName.Equals(loginDto.UserName.ToLower()));

         // => if UserName is not provided check for Email
         user ??= await _userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(loginDto.UserName.ToLower()));

         if (user == null) return BadRequest("User not found.");

         // The returned result is boolean 
         var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

         if (!result) return Unauthorized("Email or password incorrect.");

         var userDto = await UserDto(user);

         return Ok(userDto);
      }

      [AllowAnonymous]
      [HttpPost("register")]
      public async Task<IActionResult> Register(RegisterDto dto)
      {
         return await RegisterInternal(
            dto.Email,
            dto.UserName,
            dto.Password,
            ["User"]
         );
      }

      [Authorize(Policy = "RequireAdminRole")]
      [HttpPost("register/admin")]
      public async Task<IActionResult> RegisterAdmin(RegisterProtectedDto dto)
      {
         return await RegisterInternal(
            dto.Email,
            dto.UserName,
            dto.Password,
            dto.Roles
         );
      }

      [Authorize(Policy = "RequireAdminRole")]
      [HttpGet("user")]
      public async Task<IActionResult> GetUser()
      {
         var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

         var userDto = await UserDto(user);

         return Ok(userDto);
      }

      [Authorize(Policy = "RequireAdminRole")]
      [HttpPost("change-password")]
      public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
      {
         var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

         if (user == null) return NotFound("User not found");

         var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

         if (!result.Succeeded)
         {
            foreach (var error in result.Errors)
            {
               return Unauthorized(error.Description);
            }
         }

         var userDto = await UserDto(user);

         return Ok(userDto);
      }

      #region Private methods
      private async Task<UserDto> UserDto(AppUser user)
      {
         var roles = await _userManager.GetRolesAsync(user);

         var userDto = new UserDto
         {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Token = await _tokenService.CreateTokenAsync(user),
            Roles = [.. roles],
         };

         return userDto;
      }

      private async Task<IActionResult> RegisterInternal(string email, string userName, string password, IEnumerable<string> roles)
      {
         if (await _userManager.Users.AnyAsync(x => x.Email.Equals(email.ToLower())))
            return BadRequest("User exists");

         if (await _userManager.Users.AnyAsync(x => x.UserName.Equals(userName.ToLower())))
            return BadRequest("UserName taken");

         var user = new AppUser
         {
            Email = email.ToLower(),
            UserName = userName.ToLower(),
         };

         var result = await _userManager.CreateAsync(user, password);

         if (!result.Succeeded)
         {
            var error = result.Errors.FirstOrDefault()?.Description ?? "User creation failed";
            
            return BadRequest(error);
         }

         var rolesAdded = await AddRoles(user, roles);

         if (rolesAdded == 0) return BadRequest("No role is added");

         var userDto = await UserDto(user);

         return Ok(userDto);
      }

      private async Task<int> AddRoles(AppUser user, IEnumerable<string> roles)
      {
         var rolesToAdd = new List<AppRole>();

         foreach (var r in roles)
         {
            var role = await _roleManager.FindByNameAsync(r);

            if (role != null && !r.Equals("SuperAdmin", StringComparison.CurrentCultureIgnoreCase))
            {
               var result = await _userManager.AddToRoleAsync(user, r);

               if (result.Succeeded) rolesToAdd.Add(role);
            }
         }

         return rolesToAdd.Count;
      }
      #endregion
   }
}