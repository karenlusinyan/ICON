using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Data;
using TaskService.DTOs.Task;
using TaskService.Entities;
using TaskService.Tests.Infrastructure;

namespace TaskService.Tests;

public class TasksControllerTests : IClassFixture<TaskServiceWebFactory>
{
   private readonly TaskServiceWebFactory _factory;
   private readonly HttpClient _client;

   public TasksControllerTests(TaskServiceWebFactory factory)
   {
      _factory = factory;
      _client = factory.CreateClient();
   }

   [Fact]
   public async Task GetTasks_WithValidUser_ReturnsOkAndList()
   {
      var response = await _client.GetAsync("/api/tasks");

      response.StatusCode.Should().Be(HttpStatusCode.OK);

      var tasks = await response.Content.ReadFromJsonAsync<List<object>>();
      tasks.Should().NotBeNull();
   }

   [Fact]
   public async Task GetTask_WithUnknownId_ReturnsNotFound()
   {
      var response = await _client.GetAsync($"/api/tasks/{Guid.NewGuid()}");

      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
   }

   [Fact]
   public async Task CreateTask_WithValidData_ReturnsCreatedTask()
   {
      await SeedTaskStatus();

      var dto = new
      {
         name = "New Task",
         description = "Test description"
      };

      var response = await _client.PostAsJsonAsync("/api/tasks/create", dto);

      response.StatusCode.Should().Be(HttpStatusCode.OK);

      var created = await response.Content.ReadFromJsonAsync<TaskDto>();
      created.Should().NotBeNull();
      created.Name.Should().Be("New Task");
      created.Description.Should().Be("Test description");
   }

   [Fact]
   public async Task UpdateTask_WithValidData_ReturnsUpdatedTask()
   {
      await SeedTaskStatus();

      var createDto = new
      {
         name = "Original Task",
         description = "Original description"
      };

      var createResponse = await _client.PostAsJsonAsync("/api/tasks/create", createDto);
      createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

      var created = await createResponse.Content.ReadFromJsonAsync<TaskDto>();
      created.Should().NotBeNull();

      var completedStatus = await GetTaskStatusByCode("COMPLETED");

      var updateDto = new
      {
         id = created.Id,
         name = "Updated Task",
         description = "Updated description",
         statusCode = completedStatus.Code
      };

      var updateResponse = await _client.PutAsJsonAsync("/api/tasks/update", updateDto);

      updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

      var updated = await updateResponse.Content.ReadFromJsonAsync<TaskDto>();
      updated.Should().NotBeNull();
      updated!.Name.Should().Be("Updated Task");
      updated.Description.Should().Be("Updated description");
      updated.StatusCode.Should().Be("COMPLETED");
   }

   [Fact]
   public async Task DeleteTask_WithValidData_ReturnsOk()
   {
      await SeedTaskStatus();
      
      var createDto = new
      {
         name = "Deleting Task",
         description = "Original description"
      };

      var createResponse = await _client.PostAsJsonAsync("/api/tasks/create", createDto);
      createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

      var created = await createResponse.Content.ReadFromJsonAsync<TaskDto>();
      created.Should().NotBeNull();

      var deletedResponse = await _client.DeleteAsync($"/api/tasks/delete/{created.Id}");
      deletedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
   }

   #region Private Methods
   private async Task SeedTaskStatus()
   {
      using var scope = _factory.Services.CreateScope();

      var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();

      if (context.TaskStatuses.Any()) return;

      var statuses = new List<TaskStatusEntity>
      {
         new TaskStatusEntity
         {
            Id = Guid.NewGuid(),
            Code = "INCOMPLETE",
            Name = "Incomplete"
         },
         new TaskStatusEntity
         {
            Id = Guid.NewGuid(),
            Code = "COMPLETED",
            Name = "Completed"
         }
      };

      await context.TaskStatuses.AddRangeAsync(statuses);
      await context.SaveChangesAsync();
   }

   private async Task<TaskStatusEntity> GetTaskStatusByCode(string code)
   {
      using var scope = _factory.Services.CreateScope();

      var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();

      var taskStatuses = await context.TaskStatuses.ToListAsync();

      return taskStatuses.First(ts => ts.Code == code);
   }
   #endregion
}
