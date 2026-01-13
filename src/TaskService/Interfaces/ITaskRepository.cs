
using TaskService.Entities;

namespace TaskService.Interfaces
{
   public interface ITaskRepository
   {
      Task<List<TaskEntity>> GetTasksAsync();
      Task<TaskEntity> GetAsync(Guid id);
      Task AddAsync(TaskEntity entity);
      void Update(TaskEntity entity);
      void Remove(TaskEntity entity);
   }
}