using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Security.Interfaces;
using Security.Services;

namespace Security.Extensions
{
   public static class IdentityServiceExtensions
   {
      ////////////////////////////////////////////////////////////////////////////
      ////////////////////////// Identity without EF /////////////////////////////
      ////////////////////////////////////////////////////////////////////////////
      /// <summary>
      /// Registers only authentication, role policies, and system policies,
      /// without EF Core Identity.
      /// </summary>
      public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
      {
         //-----------------------------------------------------------------------
         // => Authentication + Authorization
         //-----------------------------------------------------------------------
         return services.AddServiceAuthentication(config);
      }
      ////////////////////////////////////////////////////////////////////////////
      ////////////////////////////////////////////////////////////////////////////

      ////////////////////////////////////////////////////////////////////////////
      /////////////////////////// Identity + EF Core /////////////////////////////
      ////////////////////////////////////////////////////////////////////////////
      /// <summary>
      /// Registers Identity services with EF Core + JWT authentication,
      /// role policies, and system service/job policies.
      /// </summary>
      public static IServiceCollection AddIdentityServices<TUser, TRole, TContext>(this IServiceCollection services, IConfiguration config)
         where TUser : IdentityUser<Guid>
         where TRole : IdentityRole<Guid>
         where TContext : DbContext
      {
         //-----------------------------------------------------------------------
         // => Identity Core + EF Stores
         //-----------------------------------------------------------------------
         services.AddIdentityCore<TUser>(options =>
         {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
         })
         .AddRoles<TRole>()
         .AddEntityFrameworkStores<TContext>()
         .AddDefaultTokenProviders();
         //-----------------------------------------------------------------------

         //-----------------------------------------------------------------------
         // => Authentication + Authorization
         //-----------------------------------------------------------------------
         services.AddServiceAuthentication(config);
         //-----------------------------------------------------------------------

         //-----------------------------------------------------------------------
         // => Regitser UserToken creation service
         //-----------------------------------------------------------------------
         services.AddScoped<IUserTokenService<TUser>, UserTokenService<TUser>>();
         //-----------------------------------------------------------------------

         return services;
      }
      ////////////////////////////////////////////////////////////////////////////
      ////////////////////////////////////////////////////////////////////////////

      ////////////////////////////////////////////////////////////////////////////
      /////////////////////////////// For APIs //////////////////////////////////
      ////////////////////////////////////////////////////////////////////////////
      /// <summary>
      /// Adds authentication (JWT Bearer), role policies
      /// </summary>
      private static IServiceCollection AddServiceAuthentication(this IServiceCollection services, IConfiguration config)
      {
         // => Add Authentication (Jwt)
         services.AddAuthentication(options =>
         {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         })
         .AddJwtBearer(options =>
         {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
               ValidateIssuer = false,
               ValidateAudience = false,
            };
         });

         // => Add Authorization (Policy-based)
         services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", p => p.RequireRole("SuperAdmin", "Admin"))
            .AddPolicy("RequireUserRole", p => p.RequireRole("SuperAdmin", "Admin", "User"));

         return services;
      }
      ////////////////////////////////////////////////////////////////////////////
      ////////////////////////////////////////////////////////////////////////////
   }
}
