using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Security.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Security.Services
{
   public abstract class TokenServiceBase<T> : ITokenService<T>
   {
      private readonly IConfiguration _config;

      public TokenServiceBase(IConfiguration config)
      {
         _config = config;
      }

      protected string GenerateToken(IEnumerable<Claim> claims, int expiresInHours = 1)
      {
         var tokenKey = _config["TokenKey"] ?? throw new Exception("TokenKey not found.");
         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

         var descriptor = new SecurityTokenDescriptor
         {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(expiresInHours),
            SigningCredentials = creds
         };

         var handler = new JwtSecurityTokenHandler();
         var token = handler.CreateToken(descriptor);
         return handler.WriteToken(token);
      }

      public abstract Task<string> CreateTokenAsync(T input);
   }
}
