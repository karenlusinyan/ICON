using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TaskService.Data;

namespace TaskService.Tests.Infrastructure;

public sealed class TaskServiceWebFactory : WebApplicationFactory<Program>
{
   protected override void ConfigureWebHost(IWebHostBuilder builder)
   {
      builder.ConfigureServices(services =>
      {
         // => Remove existing HostedService registration
         services.RemoveAll<IHostedService>();

         // => Remove existing DbContextOptions<TaskDbContext> registration
         services.RemoveAll<DbContextOptions<TaskDbContext>>();

         // => Register In-Memory Database for testing
         services.AddDbContext<TaskDbContext>(options =>
         {
            options.UseInMemoryDatabase("TaskService_TestDb");
         });

         // => Add Test Authentication Scheme
         services.AddAuthentication(options =>
         {
            options.DefaultAuthenticateScheme = TestAuthHandler.Scheme;
            // options.DefaultChallengeScheme = TestAuthHandler.Scheme;
         })
         .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
            TestAuthHandler.Scheme, _ => { });
      });
   }
}
