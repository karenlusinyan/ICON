using System.Data.Common;
using AuthService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace AuthService.Data
{
   public class AuthDbHostedService : IHostedService
   {
      private readonly IServiceProvider _serviceProvider;
      private readonly ILogger<AuthDbHostedService> _logger;

      public AuthDbHostedService(IServiceProvider serviceProvider, ILogger<AuthDbHostedService> logger)
      {
         _serviceProvider = serviceProvider;
         _logger = logger;
      }

      public async Task StartAsync(CancellationToken cancellationToken)
      {
         await InitDbAsync(cancellationToken);
      }

      public Task StopAsync(CancellationToken cancellationToken)
      {
         // No-op
         return Task.CompletedTask;
      }

      private async Task InitDbAsync(CancellationToken cancellationToken)
      {
         // Wrap the scope to dispose later
         using var scope = _serviceProvider.CreateScope();

         // Get Services from scope 
         var services = scope.ServiceProvider;

         // Get DbContext from service
         var context = services.GetRequiredService<AuthDbContext>();

         // Get UserManager and RoleManager from service
         var userManager = services.GetRequiredService<UserManager<AppUser>>();
         var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

         //-----------------------------------------------------------------------
         // => Wrapped in retry policy
         //-----------------------------------------------------------------------
         var policy = GetRetryPolicy(_logger, cancellationToken);
         await policy.ExecuteAsync(async () =>
         {
            // Check and migrate database
            await context.Database.MigrateAsync();

            // If database is empty then populate with very first user: superadmin
            await SeedData(context, userManager, roleManager, _logger);
         });
         //-----------------------------------------------------------------------
      }

      private async static Task SeedData(AuthDbContext context,
          UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ILogger<AuthDbHostedService> logger)
      {
         if (roleManager.Roles.Any())
            logger.LogInformation("Already have roles - no need to seed");

         if (userManager.Users.Any())
            logger.LogInformation("Already have users - no need to seed");

         // Check if any Roles are present
         if (!roleManager.Roles.Any())
         {
            var roles = new List<AppRole> {
               new() {
                  Name = "SuperAdmin"
               },
               new() {
                  Name = "Admin"
               },
               new() {
                  Name = "User"
               },
            };

            foreach (var role in roles)
            {
               logger.LogInformation($"role: {role} is added");
               await roleManager.CreateAsync(role);
            }
         }

         // Check if any Users are present
         if (!userManager.Users.Any())
         {
            var user = new AppUser
            {
               UserName = "superadmin",
               Email = "superadmin@test.com",
               EmailConfirmed = true,
            };
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRolesAsync(user, ["SuperAdmin"]);
         }
      }

      private static AsyncRetryPolicy GetRetryPolicy(ILogger<AuthDbHostedService> logger, CancellationToken cancellationToken)
         => Policy
            .Handle<DbException>()
            .Or<TimeoutException>()
            .WaitAndRetryForeverAsync(
               retryAttempt =>
               {
                  logger.LogWarning("Retrying database connection... {Attempt}.", retryAttempt);
                  return TimeSpan.FromSeconds(3);
               },
               onRetryAsync: async (exception, timeSpan, context) =>
               {
                  switch (exception)
                  {
                     case DbException e:
                        logger.LogError(e, "Database connection failed. Retrying in {Delay}.", timeSpan);
                        break;

                     case TimeoutException e:
                        logger.LogError(e, "Database operation timed out. Retrying in {Delay}.", timeSpan);
                        break;

                     default:
                        logger.LogError(exception, "Unknown error. Retrying in {Delay}.", timeSpan);
                        break;
                  }

                  if (cancellationToken.IsCancellationRequested)
                  {
                     logger.LogInformation("Application is stopping. Cancelling retry attempts.");
                     throw new OperationCanceledException(cancellationToken);
                  }

                  await Task.CompletedTask;
               });
   }
}
