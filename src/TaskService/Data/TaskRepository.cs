using Microsoft.EntityFrameworkCore;
using TaskService.Entities;
using TaskService.Interfaces;

namespace TaskService.Data
{
   public class TaskRepository : ITaskRepository
   {
      private readonly TaskDbContext _context;

      public TaskRepository(TaskDbContext context)
      {
         _context = context;
      }

      public async Task<List<TaskEntity>> GetAsync()
         => await _context.Tasks
            .Include(t => t.Status)
            .AsSplitQuery()
            .ToListAsync();

      public async Task<TaskEntity> GetAsync(Guid id)
         => await _context.Tasks.FindAsync(id);

      public async Task AddAsync(TaskEntity entity)
         => await _context.Tasks.AddAsync(entity);

      public void Update(TaskEntity entity)
         => _context.Tasks.Update(entity);

      public void Remove(TaskEntity entity)
         => _context.Tasks.Remove(entity);
   }
}