using AutoMapper;
using TaskService.DTOs.Task;
using TaskService.DTOs.TaskStatus;
using TaskService.Entities;

namespace TaskService.Mapper
{
   public class MappingProfiles : Profile
   {
      public MappingProfiles()
      {
         // => Task mapping
         CreateMap<TaskEntity, TaskDto>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
            .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status.Code));

         CreateMap<TaskDto, TaskEntity>();

         // => TaskStatus mapping
         CreateMap<TaskStatusEntity, TaskStatusDto>();
      }
   }
}