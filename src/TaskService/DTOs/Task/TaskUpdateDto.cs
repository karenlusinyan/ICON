using System.ComponentModel.DataAnnotations;

namespace TaskService.DTOs.Task
{
   public class TaskUpdateDto
   {
      [Required]
      public Guid Id { get; set; }
      [Required]
      public string Name { get; set; }
      public string Description { get; set; }
      [Required]
      public Guid StatusId { get; set; }
   }
}