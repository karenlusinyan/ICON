namespace TaskService.Entities
{
   public class TaskEntity
   {
      public Guid Id { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
      public bool Disabled { get; set; } = false;
      public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
      public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
      public bool Deleted { get; set; } = false;
      public Guid StatusId { get; set; }
      public Status Status { get; set; }
   }
}