using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.Auth
{
   public class RegisterDto
   {
      [Required, EmailAddress]
      public string Email { get; set; } = null!;
      [Required]
      public string UserName { get; set; } = null!;
      [Required]
      public string Password { get; set; } = null!;
   }
}