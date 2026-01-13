using Microsoft.EntityFrameworkCore;

namespace TaskService.Data
{
   public class TaskDbContext : DbContext
   {
      private readonly ILogger<TaskDbContext> _logger;

      public TaskDbContext(DbContextOptions options, ILogger<TaskDbContext> logger)
         : base(options)
      {
         _logger = logger;
         _logger.LogInformation("=> Init TaskDbContext");
      }

      public DbSet<Entities.Task> Tasks { get; set; }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         base.OnModelCreating(builder);
      }
   }
}
