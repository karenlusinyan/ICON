using AuthService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
   public class AuthDbContext : IdentityDbContext<
      AppUser,
      AppRole,
      Guid,
      IdentityUserClaim<Guid>,
      AppUserRole,
      IdentityUserLogin<Guid>,
      IdentityRoleClaim<Guid>,
      IdentityUserToken<Guid>
   >
   {
      private readonly ILogger<AuthDbContext> _logger;

      public AuthDbContext(DbContextOptions options, ILogger<AuthDbContext> logger)
         : base(options)
      {
         _logger = logger;
         _logger.LogInformation("=> Init AuthDbContext");
      }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         base.OnModelCreating(builder);

         builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

         builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(r => r.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();
      }
   }
}
