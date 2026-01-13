namespace TaskService.Entities
{
   public enum TaskStatus
   {
      Unknown,
      Complete,
      Incomplete
   }

   public class Task
   {
      public Guid Id { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
      public TaskStatus Status { get; set; }
      public bool Disabled { get; set; }
      public DateTime CreatedAt { get; set; }
      public DateTime ModifiedAt { get; set; }
      public bool Deleted { get; set; }
   }
}