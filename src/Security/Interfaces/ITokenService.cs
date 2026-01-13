namespace Security.Interfaces
{
   public interface ITokenService<T>
   {
      Task<string> CreateTokenAsync(T input);
   }
}
