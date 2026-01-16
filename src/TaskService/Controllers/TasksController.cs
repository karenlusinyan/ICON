using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.DTOs.Task;
using TaskService.Entities;
using TaskService.Interfaces;
using TaskService.Request;

namespace TaskService.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   // [Route("api/v{version:apiVersion}/[controller]")]
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
      [MapToApiVersion("1.0")]
      public async Task<IActionResult> GetTasks([FromQuery] TaskFilters filters)
      {
         // var tasks = await _unitOfWork.TaskRepository.GetAsync(filters);
         // return Ok(_mapper.Map<List<TaskDto>>(tasks));

         // ----------------------------------------------------------------------
         // => Raw-SQL version
         // ----------------------------------------------------------------------
         var tasks = await _unitOfWork.TaskRepository.GetSqlAsync(filters);
         return Ok(tasks);
         // ----------------------------------------------------------------------
      }

      [Authorize(Policy = "RequireUserRole")]
      [HttpGet("{id}")]
      public async Task<IActionResult> GetTask([FromRoute] Guid id)
      {
         var task = await _unitOfWork.TaskRepository.GetAsync(id);
         if (task == null) return NotFound("Task not found");

         return Ok(_mapper.Map<TaskDto>(task));

         // ----------------------------------------------------------------------
         // => Raw-SQL version
         // ----------------------------------------------------------------------
         // var task = await _unitOfWork.TaskRepository.GetSqlAsync(id);
         // if (task == null) return NotFound("Task not found");

         // return Ok(task);
         // ----------------------------------------------------------------------
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

         // ----------------------------------------------------------------------
         // => Set default status to "INCOMPLETE"
         // ----------------------------------------------------------------------
         var defaultStatus = await _unitOfWork.StatusRepository.GetByCodeAsync(TaskStatusCode.INCOMPLETE.ToString());
         task.StatusId = defaultStatus.Id;
         // ----------------------------------------------------------------------

         _unitOfWork.TaskRepository.Add(task);

         if (await _unitOfWork.CommitAsync() > 0)
         {
            var insertedTask = await _unitOfWork.TaskRepository.GetAsync(task.Id);
            return Ok(_mapper.Map<TaskDto>(insertedTask));
         }

         return BadRequest("Can not create Task, please try again!");

         // ----------------------------------------------------------------------
         // => Raw-SQL version
         // ----------------------------------------------------------------------
         // await _unitOfWork.TaskRepository.InsertSqlAsync(task);
         // var insertedTask = await _unitOfWork.TaskRepository.GetSqlAsync(task.Id);

         // return Ok(insertedTask);
         // ----------------------------------------------------------------------
      }

      [Authorize(Policy = "RequireUserRole")]
      [HttpPut("update")]
      public async Task<IActionResult> UpdateTask([FromBody] TaskUpdateDto updateDto)
      {
         var task = await _unitOfWork.TaskRepository.GetAsync(updateDto.Id);
         if (task == null) return NotFound("Task not found");

         task.Name = updateDto.Name;
         task.Description = updateDto.Description;

         // ----------------------------------------------------------------------
         // => Update status
         // ----------------------------------------------------------------------
         var status = await _unitOfWork.StatusRepository.GetByCodeAsync(updateDto.StatusCode.ToString());
         if (status == null) return NotFound("Status not found");

         task.StatusId = status.Id;
         // ----------------------------------------------------------------------

         _unitOfWork.TaskRepository.Update(task);

         if (await _unitOfWork.CommitAsync() > 0)
         {
            var updatedTask = await _unitOfWork.TaskRepository.GetAsync(task.Id);
            return Ok(_mapper.Map<TaskDto>(updatedTask));
         }

         return BadRequest("Can not update Task, please try again!");

         // ----------------------------------------------------------------------
         // => Raw-SQL version
         // ----------------------------------------------------------------------
         // await _unitOfWork.TaskRepository.UpdateSqlAsync(task);
         // var updatedTask = await _unitOfWork.TaskRepository.GetSqlAsync(task.Id);

         // return Ok(updatedTask);
         // ----------------------------------------------------------------------
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

         // ----------------------------------------------------------------------
         // => Raw-SQL version
         // ----------------------------------------------------------------------
         // await _unitOfWork.TaskRepository.DeleteSqlAsync(id);

         // return Ok("Task deleted successfuly");
         // ----------------------------------------------------------------------
      }
   }
}