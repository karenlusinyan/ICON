using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TaskService.DTOs.Task;
using TaskService.Entities;
using TaskService.Interfaces;
using TaskService.Request;

namespace TaskService.Data
{
   public class TaskRepository : ITaskRepository
   {
      private readonly TaskDbContext _context;

      public TaskRepository(TaskDbContext context)
      {
         _context = context;
      }

      #region EF-tracked operations
      public async Task<List<TaskEntity>> GetAsync(TaskFilters filters)
      {
         IQueryable<TaskEntity> query = _context.Tasks
            .Include(t => t.Status)
            .AsSplitQuery();

         if (filters.StatusId.HasValue)
         {
            query = query.Where(t => t.StatusId == filters.StatusId);
         }

         return await query
            .OrderBy(t => t.OrderIndex)
            .ThenByDescending(t => t.CreatedAt)
            .ToListAsync();
      }

      public async Task<List<TaskEntity>> GetAsyncByIds(List<Guid> ids)
         => await _context.Tasks
            .Where(t => ids.Contains(t.Id))
            .ToListAsync();

      public async Task<TaskEntity> GetAsync(Guid id)
         => await _context.Tasks
            .Include(t => t.Status)
            .AsSplitQuery()
            .FirstOrDefaultAsync(t => t.Id == id);

      public void Add(TaskEntity entity)
         => _context.Tasks.Add(entity);

      public void Update(TaskEntity entity)
         => _context.Tasks.Update(entity);

      public void Remove(TaskEntity entity)
         => _context.Tasks.Remove(entity);

      public async Task ReorderTasksAsync(Dictionary<Guid, int> orderMap)
      {
         var tasks = await GetAsyncByIds([.. orderMap.Keys]);

         foreach (var task in tasks)
         {
            task.OrderIndex = orderMap[task.Id];
         }
      }
      #endregion

      #region Raw-SQL operations
      public async Task<List<TaskDto>> GetSqlAsync(TaskFilters filters)
      {
         var statusId = filters.StatusId;

         return await _context.Database
            .SqlQuery<TaskDto>(
               $"""
               SELECT
                  t.Id,
                  t.Name,
                  t.Description,
                  t.OrderIndex,
                  t.StatusId,
                  t.Disabled,
                  t.CreatedAt,
                  t.ModifiedAt,
                  t.Deleted,
                  s.Code AS StatusCode,
                  s.Name AS StatusName
               FROM Tasks t
               INNER JOIN TaskStatuses s ON s.Id = t.StatusId
               WHERE ({statusId} IS NULL OR t.StatusId = {statusId})
               ORDER BY t.OrderIndex ASC, t.CreatedAt DESC
               """
            )
            .ToListAsync();
      }

      public async Task<List<TaskDto>> GetSqlAsyncByIds(List<Guid> ids)
      {
         var json = JsonSerializer.Serialize(ids);

         return await _context.Database.SqlQueryRaw<TaskDto>(
            """
            SELECT
               t.Id,
               t.Name,
               t.Description,
               t.OrderIndex,
               t.StatusId,
               t.Disabled,
               t.CreatedAt,
               t.ModifiedAt,
               t.Deleted,
               s.Code AS StatusCode,
               s.Name AS StatusName
            FROM Tasks t
            INNER JOIN TaskStatuses s ON s.Id = t.StatusId
            WHERE t.Id IN (
               SELECT value FROM OPENJSON(@ids)
            )
            """,
            new SqlParameter("@ids", json)
         )
         .ToListAsync();
      }

      public async Task<TaskDto> GetSqlAsync(Guid id)
      {
         return await _context.Database
            .SqlQuery<TaskDto>(
              $"""
               SELECT
                  t.Id,
                  t.Name,
                  t.Description,
                  t.OrderIndex,
                  t.StatusId,
                  t.Disabled,
                  t.CreatedAt,
                  t.ModifiedAt,
                  t.Deleted,
                  s.Code AS StatusCode,
                  s.Name AS StatusName
               FROM Tasks t
               INNER JOIN TaskStatuses s ON s.Id = t.StatusId
               WHERE t.Id = {id}
               """
            )
            .FirstOrDefaultAsync();
      }

      public async Task InsertSqlAsync(TaskEntity entity)
      {
         await _context.Database.ExecuteSqlInterpolatedAsync(
            $"""
            INSERT INTO Tasks (
               Id,
               Name,
               Description,
               StatusId,
               Disabled,
               CreatedAt,
               ModifiedAt,
               Deleted
            )
            VALUES (
               {entity.Id},
               {entity.Name},
               {entity.Description},
               {entity.StatusId},
               {entity.Disabled},
               {entity.CreatedAt},
               {entity.ModifiedAt},
               {entity.Deleted}
            )
            """
         );
      }

      public async Task UpdateSqlAsync(TaskEntity entity)
      {
         await _context.Database.ExecuteSqlInterpolatedAsync(
            $"""
            UPDATE Tasks
            SET
               Name = {entity.Name},
               Description = {entity.Description},
               StatusId = {entity.StatusId}
            WHERE Id = {entity.Id}
            """
         );
      }

      public async Task DeleteSqlAsync(Guid id)
      {
         await _context.Database.ExecuteSqlInterpolatedAsync(
            $"""
            DELETE FROM Tasks
            WHERE Id = {id}
            """
         );
      }

      public async Task ReorderTasksSqlAsync(Dictionary<Guid, int> orderMap)
      {
         var json = JsonSerializer.Serialize(orderMap);

         await _context.Database.ExecuteSqlRawAsync(
            """
            UPDATE t
            SET t.OrderIndex = j.[value]
            FROM Tasks t
            INNER JOIN OPENJSON(@map) j
               ON CAST(j.[key] AS uniqueidentifier) = t.Id
            """,
            new SqlParameter("@map", json)
         );
      }
      #endregion
   }
}