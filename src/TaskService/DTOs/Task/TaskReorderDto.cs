namespace TaskService.DTOs.Task
{
   public class TaskOrderDto
   {
      public Guid Id { get; set; }
      public int OrderIndex { get; set; }
   }

   public class TaskReorderDto
   {
      public ICollection<TaskOrderDto> Tasks { get; set; } = [];
   }
}