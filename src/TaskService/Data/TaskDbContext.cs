using Microsoft.EntityFrameworkCore;
using TaskService.Entities;

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

      public DbSet<TaskEntity> Tasks { get; set; }
      public DbSet<TaskStatusEntity> TaskStatuses { get; set; }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         base.OnModelCreating(builder);

         builder.Entity<TaskStatusEntity>()
            .HasMany(s => s.Tasks)
            .WithOne(t => t.Status)
            .HasForeignKey(t => t.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
      }
   }
}
