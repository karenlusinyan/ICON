using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Buffers;
using System.Text;

namespace AuthService.Extensions
{
   public static class DbContextExtensions
   {
      public static IServiceCollection AddDbContext<TContext>(this IServiceCollection services, IConfiguration config, string connectionString)
         where TContext : DbContext
      {
         // => Register DbContext
         services.AddDbContext<TContext>(options =>
         {
            options.UseSqlServer(config.GetConnectionString(connectionString));
         });

         return services;
      }
   }
}
