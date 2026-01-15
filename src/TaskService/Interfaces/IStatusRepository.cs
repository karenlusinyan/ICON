
using TaskService.Entities;

namespace TaskService.Interfaces
{
   public interface IStatusRepository
   {
      Task<List<TaskStatusEntity>> GetAsync();
      Task<TaskStatusEntity> GetAsync(Guid id);
      Task<TaskStatusEntity> GetByCodeAsync(string code);
   }
}