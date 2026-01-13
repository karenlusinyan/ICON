using TaskService.Interfaces;

namespace TaskService.Data
{
   public class UnitOfWork : IUnitOfWork
   {
      private readonly TaskDbContext _context;
      public ITaskRepository TaskRepository { get; }
      public IStatusRepository StatusRepository { get; }

      public UnitOfWork(TaskDbContext context, ITaskRepository taskRepository, IStatusRepository statusRepository)
      {
         _context = context;

         TaskRepository = taskRepository;
         StatusRepository = statusRepository;
      }

      public async Task<int> CommitAsync()
          => await _context.SaveChangesAsync();
   }
}