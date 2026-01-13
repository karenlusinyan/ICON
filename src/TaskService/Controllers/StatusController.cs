using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.DTOs.Status;
using TaskService.Interfaces;

namespace TaskService.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   [Route("api/v{version:apiVersion}/[controller]")]
   [ApiVersion("1.0")]
   [ApiVersion("2.0")]
   public class StatusController : ControllerBase
   {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;
      public StatusController(IUnitOfWork unitOfWork, IMapper mapper)
      {
         _unitOfWork = unitOfWork;
         _mapper = mapper;
      }

      [Authorize(Policy = "RequireUserRole")]
      [HttpGet]
      [MapToApiVersion("1.0")]
      public async Task<IActionResult> GetStatuses()
      {
         var statuses = await _unitOfWork.StatusRepository.GetStatusesAsync();

         return Ok(_mapper.Map<List<StatusDto>>(statuses));
      }

      [Authorize(Policy = "RequireUserRole")]
      [HttpGet("{id}")]
      public async Task<IActionResult> GetStatus([FromRoute] Guid id)
      {
         var status = await _unitOfWork.StatusRepository.GetAsync(id);
         if (status == null) return NotFound("Status not found");

         return Ok(_mapper.Map<StatusDto>(status));
      }
   }
}