using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions
{
   public static class DbContextExtensions
   {
      public static IServiceCollection AddDbContext<TContext>(
         this IServiceCollection services,
         IConfiguration configuration,
         string connectionStringName
      )
         where TContext : DbContext
      {
         var cs = configuration.GetConnectionString(connectionStringName);

         if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException($"Connection string '{connectionStringName}' not found");

         Console.WriteLine($"[DB-CONFIG] {typeof(TContext).Name} => {connectionStringName}");

         services.AddDbContext<TContext>(options =>
            options.UseSqlServer(cs)
         );

         return services;
      }
   }
}
