namespace TaskService.Entities
{
   public class Status
   {
      public Guid Id { get; set; }
      public string Name { get; set; }
      public ICollection<TaskEntity> Tasks { get; set; }
   }
}