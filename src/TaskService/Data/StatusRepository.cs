using Microsoft.EntityFrameworkCore;
using TaskService.Entities;
using TaskService.Interfaces;

namespace TaskService.Data
{
   public class StatusRepository : IStatusRepository
   {
      private readonly TaskDbContext _context;

      public StatusRepository(TaskDbContext context)
      {
         _context = context;
      }

      public async Task<List<TaskStatusEntity>> GetAsync()
         => await _context.TaskStatuses.ToListAsync();

      public async Task<TaskStatusEntity> GetAsync(Guid id)
         => await _context.TaskStatuses.FindAsync(id);

      public async Task<TaskStatusEntity> GetByCodeAsync(string code)
         => await _context.TaskStatuses.FirstOrDefaultAsync(s => s.Code == code);
   }
}