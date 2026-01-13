namespace TaskService.DTOs.Task
{
   public class TaskDto
   {
      public Guid Id { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
      public Guid StatusId { get; set; }
      public bool Disabled { get; set; }
      public DateTime CreatedAt { get; set; }
      public DateTime ModifiedAt { get; set; }
      public bool Deleted { get; set; }
   }
}