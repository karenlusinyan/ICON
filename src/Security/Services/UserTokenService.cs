using Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Security.Services
{
   public class UserTokenService<TUser> : TokenServiceBase<TUser>, IUserTokenService<TUser>
      where TUser : IdentityUser<Guid>
   {
      private readonly UserManager<TUser> _userManager;

      public UserTokenService(IConfiguration config, UserManager<TUser> userManager) : base(config)
      {
         _userManager = userManager;
      }

      public override async Task<string> CreateTokenAsync(TUser user)
      {
         var roles = await _userManager.GetRolesAsync(user);

         var claims = new List<Claim>
         {
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new("Username", user.UserName ?? string.Empty)
         };

         claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

         return GenerateToken(claims, 8760); // ~1 year
      }
   }
}