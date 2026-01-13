using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.DTOs.Task;
using TaskService.Entities;
using TaskService.Interfaces;

namespace TaskService.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   [ApiVersion("1.0")]
   [ApiVersion("2.0")]
   public class TasksController : ControllerBase
   {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;
      public TasksController(IUnitOfWork unitOfWork, IMapper mapper)
      {
         _unitOfWork = unitOfWork;
         _mapper = mapper;
      }

      [Authorize(Policy = "RequireUserRole")]
      [HttpGet]
      public async Task<IActionResult> GetTasks()
      {
         var tasks = await _unitOfWork.TaskRepository.GetTasksAsync();

         return Ok(_mapper.Map<List<TaskDto>>(tasks));
      }

      [Authorize(Policy = "RequireUserRole")]
      [HttpGet("{id}")]
      public async Task<IActionResult> GetTask([FromRoute] Guid id)
      {
         var task = await _unitOfWork.TaskRepository.GetAsync(id);
         if (task == null) return NotFound("Task not found");

         return Ok(_mapper.Map<TaskDto>(task));
      }

      [Authorize(Policy = "RequireUserRole")]
      [HttpPost("create")]
      public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto createDto)
      {
         var task = new TaskEntity
         {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            Description = createDto.Description,
         };

         await _unitOfWork.TaskRepository.AddAsync(task);

         if (await _unitOfWork.CommitAsync() > 0)
         {
            var insertedTask = await _unitOfWork.TaskRepository.GetAsync(task.Id);
            return Ok(_mapper.Map<TaskDto>(insertedTask));
         }

         return BadRequest("Can not create Task, please try again!");
      }


      [Authorize(Policy = "RequireUserRole")]
      [HttpPut("update/{id}")]
      public async Task<IActionResult> UpdateTask([FromBody] TaskUpdateDto updateDto, Guid id)
      {
         var task = await _unitOfWork.TaskRepository.GetAsync(id);
         if (task == null) return NotFound("Task not found");

         var status = await _unitOfWork.StatusRepository.GetAsync(updateDto.StatusId);
         if (status == null) return NotFound("Status not found");

         task.Name = updateDto.Name;
         task.Description = updateDto.Description;
         task.StatusId = updateDto.StatusId;

         _unitOfWork.TaskRepository.Update(task);

         if (await _unitOfWork.CommitAsync() > 0)
         {
            var updatedTask = await _unitOfWork.TaskRepository.GetAsync(task.Id);
            return Ok(_mapper.Map<TaskDto>(updatedTask));
         }

         return BadRequest("Can not update Task, please try again!");
      }

      [Authorize(Policy = "RequireUserRole")]
      [HttpDelete("delete/{id}")]
      public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
      {
         var task = await _unitOfWork.TaskRepository.GetAsync(id);
         if (task == null) return NotFound("Task not found");

         _unitOfWork.TaskRepository.Remove(task);

         if (await _unitOfWork.CommitAsync() > 0)
         {
            return Ok("Task deleted successfuly");
         }

         return BadRequest("Can not delete Task, please try again!");
      }
   }
}