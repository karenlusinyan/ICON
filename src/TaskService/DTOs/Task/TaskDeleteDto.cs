using System.ComponentModel.DataAnnotations;

namespace TaskService.DTOs.Task
{
   public class TaskDeleteDto
   {
      [Required]
      public Guid Id { get; set; }
   }
}