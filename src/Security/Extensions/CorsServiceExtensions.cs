using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Security.Extensions
{
   public static class CorsServiceExtensions
   {
      public static IServiceCollection AddCorsServices(this IServiceCollection services, IConfiguration configuration)
      {
         // ----------------------------------------------------------------------
         // IMPORTANT: CORS configuration
         // ----------------------------------------------------------------------
         // Only host.docker.internal is included in the allowed origins through
         // configuration["ClientAppUrl"]. "http://localhost:3000" is intentionally
         // excluded because both localhost and host.docker.internal resolve to
         // 127.0.0.1 (IPv4). This mirrors the frontend .env setup, enforcing a
         // single base URL across environments to ensure consistency and prevent
         // switching between development and production modes, as well as between
         // running services locally or inside containers.
         // ----------------------------------------------------------------------
         var origins = new List<string>{
            configuration["ClientAppUrl"]
         };
         Console.WriteLine($"CORS allowing origins => {string.Join(", ", origins)}");

         services.AddCors(options =>
         {
            options.AddDefaultPolicy(policy =>
            {
               policy.AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials()
                     .WithOrigins([.. origins]);
            });
         });

         return services;
      }
   }
}
