using System.ComponentModel.DataAnnotations;

namespace TaskService.DTOs.Task
{
   public class TaskCreateDto
   {
      [Required]
      public string Name { get; set; }
      public string Description { get; set; }
   }
}