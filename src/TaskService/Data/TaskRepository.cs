using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TaskService.DTOs.Task;
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

      #region EF-tracked operations
      public async Task<List<TaskEntity>> GetAsync()
         => await _context.Tasks
            .Include(t => t.Status)
            .OrderByDescending(t => t.CreatedAt)
            .AsSplitQuery()
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
      #endregion

      #region Raw-SQL operations
      public async Task<List<TaskDto>> GetSqlAsync()
      {
         return await _context.Database
            .SqlQuery<TaskDto>(
               $"""
               SELECT
                  t.Id,
                  t.Name,
                  t.Description,
                  t.StatusId,
                  t.Disabled,
                  t.CreatedAt,
                  t.ModifiedAt,
                  t.Deleted,
                  s.Code AS StatusCode,
                  s.Name AS StatusName
               FROM Tasks t
               INNER JOIN TaskStatuses s ON s.Id = t.StatusId
               ORDER BY t.CreatedAt DESC
               """
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
      #endregion
   }
}