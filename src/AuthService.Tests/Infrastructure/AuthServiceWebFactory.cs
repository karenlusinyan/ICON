using AuthService.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace AuthService.Tests.Infrastructure;

public sealed class AuthServiceWebFactory : WebApplicationFactory<Program>
{
   protected override void ConfigureWebHost(IWebHostBuilder builder)
   {
      builder.ConfigureServices(services =>
      {
         // => Remove existing HostedService registration
         services.RemoveAll<IHostedService>();

         // => Remove existing DbContextOptions<AuthDbContext> registration
         services.RemoveAll<DbContextOptions<AuthDbContext>>();

         // => Register In-Memory Database for testing
         services.AddDbContext<AuthDbContext>(options =>
         {
            options.UseInMemoryDatabase("AuthService_TestDb");
         });
      });
   }
}
