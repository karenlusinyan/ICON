using Microsoft.AspNetCore.Identity;

namespace Security.Interfaces
{
   public interface IUserTokenService<TUser> : ITokenService<TUser> where TUser : IdentityUser<Guid> { }
}