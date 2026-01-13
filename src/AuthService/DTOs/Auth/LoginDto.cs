using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.Auth
{
   public class LoginDto
   {
      [Required]
      public string UserName { get; set; } // IMPORTANT: => Email/UserName
      [Required]
      public string Password { get; set; }
   }
}