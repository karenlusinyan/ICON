namespace TaskService.Entities
{
   public enum TaskStatusCode
   {
      NEW,
      INCOMPLETE,
      COMPLETED,
      ON_HOLD,
      CANCELLED
   }

   public class TaskStatusEntity
   {
      public Guid Id { get; set; }
      public string Code { get; set; }
      public string Name { get; set; }
      public ICollection<TaskEntity> Tasks { get; set; } = [];
   }
}