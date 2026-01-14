namespace AuthService.DTOs.Auth
{
   public class RegisterProtectedDto : RegisterDto
   {
      public ICollection<string> Roles { get; set; }
   }
}