
using TaskService.DTOs.Task;
using TaskService.Entities;
using TaskService.Request;

namespace TaskService.Interfaces
{
   public interface ITaskRepository
   {
      #region EF-tracked operations
      Task<List<TaskEntity>> GetAsync(TaskFilters filters);
      Task<List<TaskEntity>> GetAsyncByIds(List<Guid> ids);
      Task<TaskEntity> GetAsync(Guid id);
      void Add(TaskEntity entity);
      void Update(TaskEntity entity);
      void Remove(TaskEntity entity);
      Task ReorderTasksAsync(Dictionary<Guid, int> orderMap);
      #endregion

      #region Raw-SQL operations
      Task<List<TaskDto>> GetSqlAsync(TaskFilters filters);
      Task<List<TaskDto>> GetSqlAsyncByIds(List<Guid> ids);
      Task<TaskDto> GetSqlAsync(Guid id);
      Task InsertSqlAsync(TaskEntity entity);
      Task UpdateSqlAsync(TaskEntity entity);
      Task DeleteSqlAsync(Guid id);
      Task ReorderTasksSqlAsync(Dictionary<Guid, int> orderMap);
      #endregion
   }
}