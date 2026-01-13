
using TaskService.Entities;

namespace TaskService.Interfaces
{
   public interface IStatusRepository
   {
      Task<List<Status>> GetStatusesAsync();
      Task<Status> GetAsync(Guid id);
   }
}