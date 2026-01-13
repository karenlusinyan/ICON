using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.Auth
{
   public class RegisterDto
   {
      [Required]
      [EmailAddress]
      public string Email { get; set; }
      [Required]
      [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")]
      public string Password { get; set; }
      [Required]
      public string UserName { get; set; }
      [Required]
      public ICollection<string> Roles { get; set; }
   }
}