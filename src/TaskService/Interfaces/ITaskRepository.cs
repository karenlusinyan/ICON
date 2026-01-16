
using TaskService.DTOs.Task;
using TaskService.Entities;

namespace TaskService.Interfaces
{
   public interface ITaskRepository
   {
      #region EF-tracked operations
      Task<List<TaskEntity>> GetAsync();
      Task<TaskEntity> GetAsync(Guid id);
      void Add(TaskEntity entity);
      void Update(TaskEntity entity);
      void Remove(TaskEntity entity);
      #endregion

      #region Raw-SQL operations
      Task<List<TaskDto>> GetSqlAsync();
      Task<TaskDto> GetSqlAsync(Guid id);
      Task InsertSqlAsync(TaskEntity entity);
      Task UpdateSqlAsync(TaskEntity entity);
      Task DeleteSqlAsync(Guid id);
      #endregion
   }
}