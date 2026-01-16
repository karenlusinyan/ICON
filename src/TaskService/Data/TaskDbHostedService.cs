using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using TaskService.Entities;

namespace TaskService.Data
{
   public class TaskDbHostedService : IHostedService
   {
      private readonly IServiceProvider _serviceProvider;
      private readonly ILogger<TaskDbHostedService> _logger;

      public TaskDbHostedService(IServiceProvider serviceProvider, ILogger<TaskDbHostedService> logger)
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
         var context = services.GetRequiredService<TaskDbContext>();

         //-----------------------------------------------------------------------
         // => Wrapped in retry policy
         //-----------------------------------------------------------------------
         var policy = GetRetryPolicy(_logger, cancellationToken);
         await policy.ExecuteAsync(async () =>
         {
            // Check and migrate database
            await context.Database.MigrateAsync();

            // If database is empty then populate with very first user: superadmin
            await SeedData(context, _logger);
         });
         //-----------------------------------------------------------------------
      }

      private async static Task SeedData(TaskDbContext context, ILogger<TaskDbHostedService> logger)
      {
         if (!context.TaskStatuses.Any())
         {
            var statuses = new List<TaskStatusEntity> {
               new() {
                  Id = Guid.NewGuid(),
                  Code = "INCOMPLETE",
                  Name = "Incomplete"
               },
               new() {
                  Id = Guid.NewGuid(),
                  Code = "COMPLETED",
                  Name = "Completed"
               },
            };

            await context.AddRangeAsync(statuses);
            await context.SaveChangesAsync();
         }
      }

      private static AsyncRetryPolicy GetRetryPolicy(ILogger<TaskDbHostedService> logger, CancellationToken cancellationToken)
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
