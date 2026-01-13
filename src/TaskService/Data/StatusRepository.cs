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

      public async Task<List<Status>> GetStatusesAsync()
         => await _context.Statuses.ToListAsync();

      public async Task<Status> GetAsync(Guid id)
         => await _context.Statuses.FindAsync(id);
   }
}