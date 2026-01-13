using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions
{
   public static class DbContextExtensions
   {
      public static IServiceCollection AddDbContext<TContext>(this IServiceCollection services, IConfiguration config, string database)
         where TContext : DbContext
      {
         // => Register DbContext
         services.AddDbContext<TContext>(options =>
         {
            options.UseSqlServer($"{config.GetConnectionString("DefaultConnection")}{database}");
         });

         return services;
      }
   }
}
