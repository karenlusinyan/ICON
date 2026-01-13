namespace TaskService.Interfaces
{
   public interface IUnitOfWork
   {
      ITaskRepository TaskRepository { get; }
      IStatusRepository StatusRepository { get; }
      Task<int> CommitAsync();
   }
}