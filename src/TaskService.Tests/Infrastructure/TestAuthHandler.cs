using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TaskService.Tests.Infrastructure;

public sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
   public new const string Scheme = "Test";

   public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
      : base(options, logger, encoder)
   {

   }

   protected override Task<AuthenticateResult> HandleAuthenticateAsync()
   {
      var claims = new[]
      {
         new Claim(ClaimTypes.NameIdentifier, "test-user"),
         new Claim(ClaimTypes.Email, "test@test.com"),
         new Claim(ClaimTypes.Role, "User")
      };

      var identity = new ClaimsIdentity(claims, Scheme);
      var principal = new ClaimsPrincipal(identity);
      var ticket = new AuthenticationTicket(principal, Scheme);

      return Task.FromResult(AuthenticateResult.Success(ticket));
   }
}
