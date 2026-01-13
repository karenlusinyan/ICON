using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions
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
