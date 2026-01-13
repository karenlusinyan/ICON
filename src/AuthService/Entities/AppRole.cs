using Microsoft.AspNetCore.Identity;

namespace AuthService.Entities
{
   // By specifing IdentiyUser<int> we specify the type of Id that is int 
   public class AppRole : IdentityRole<Guid>
   {
      public ICollection<AppUserRole> UserRoles { get; set; }
   }
}