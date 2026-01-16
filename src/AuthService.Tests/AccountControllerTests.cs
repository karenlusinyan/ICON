using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using AuthService.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using AuthService.Entities;

namespace AuthService.Tests;

public class AccountControllerTests : IClassFixture<AuthServiceWebFactory>
{
   private readonly AuthServiceWebFactory _factory;
   private readonly HttpClient _client;

   public AccountControllerTests(AuthServiceWebFactory factory)
   {
      _factory = factory;
      _client = factory.CreateClient();
   }

   [Fact]
   public async Task Login_WithUnknownUser_ReturnsBadRequest()
   {
      var dto = new
      {
         userName = "notfound@test.com",
         password = "Pa$$w0rd"
      };

      var response = await _client.PostAsJsonAsync("/api/account/login", dto);

      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
   }

   [Fact]
   public async Task Login_WithWrongPassword_ReturnsUnauthorized()
   {
      await SeedUser("user@test.com", "Pa$$w0rd");

      var dto = new
      {
         userName = "user@test.com",
         password = "WrongPassword!"
      };

      var response = await _client.PostAsJsonAsync("/api/account/login", dto);

      response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
   }

   [Fact]
   public async Task Register_WithMissingPassword_ReturnsBadRequest()
   {
      var dto = new
      {
         userName = "user@test.com",
         email = "user@test.com"
      };

      var response = await _client.PostAsJsonAsync("/api/account/register", dto);

      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
   }

   [Fact]
   public async Task Register_WithWeakPassword_ReturnsBadRequest()
   {
      var dto = new
      {
         userName = "weak@test.com",
         email = "weak@test.com",
         password = "1234"
      };

      var response = await _client.PostAsJsonAsync("/api/account/register", dto);
      var body = await response.Content.ReadAsStringAsync();

      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      body.Should().Contain("Password");
   }

   [Fact]
   public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
   {
      await SeedUser("dup@test.com", "Pa$$w0rd");

      var dto = new
      {
         userName = "dup_user",
         email = "dup@test.com",
         password = "Pa$$w0rd"
      };

      var response = await _client.PostAsJsonAsync("/api/account/register", dto);

      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
   }

   [Fact]
   public async Task Login_WithValidData_ReturnsUserDto()
   {
      await SeedUser("user@test.com", "Pa$$w0rd");

      var dto = new
      {
         userName = "user@test.com",
         password = "Pa$$w0rd"
      };

      var response = await _client.PostAsJsonAsync("/api/account/login", dto);
      var body = await response.Content.ReadAsStringAsync();

      response.StatusCode.Should().Be(HttpStatusCode.OK);
      body.Should().Contain("token");
      body.Should().Contain("email");
      body.Should().Contain("userName");
   }

   [Fact]
   public async Task Register_WithValidData_ReturnsUserDto()
   {
      await SeedRole("User");

      var dto = new
      {
         userName = "new",
         email = "new@test.com",
         password = "Pa$$w0rd"
      };

      var response = await _client.PostAsJsonAsync("/api/account/register", dto);
      var body = await response.Content.ReadAsStringAsync();

      response.StatusCode.Should().Be(HttpStatusCode.OK);
      body.Should().Contain("token");
      body.Should().Contain("email");
      body.Should().Contain("userName");
   }

   #region Private Methods
   private async Task SeedRole(string roleName)
   {
      using var scope = _factory.Services.CreateScope();

      var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

      if (await roleManager.RoleExistsAsync(roleName)) return;

      await roleManager.CreateAsync(
         new AppRole
         {
            Name = roleName
         }
      );
   }

   private async Task SeedUser(string email, string password)
   {
      using var scope = _factory.Services.CreateScope();

      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

      var exisintgUser = await userManager.FindByEmailAsync(email);
      if (exisintgUser != null) return;

      var user = new AppUser
      {
         Email = email,
         UserName = email
      };

      await userManager.CreateAsync(user, password);
   }
   #endregion
}
