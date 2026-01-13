using AutoMapper;
using TaskService.DTOs.Status;
using TaskService.DTOs.Task;
using TaskService.Entities;

namespace TaskService.Mapper
{
   public class MappingProfiles : Profile
   {
      public MappingProfiles()
      {
         // => Task mapping
         CreateMap<TaskEntity, TaskDto>();
         CreateMap<TaskDto, TaskEntity>();

         // => Status mapping
         CreateMap<Status, StatusDto>();
      }
   }
}