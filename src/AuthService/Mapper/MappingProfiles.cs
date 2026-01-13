using AuthService.DTOs.User;
using AuthService.Entities;
using AutoMapper;

namespace AuthService.Mapper
{
   public class MappingProfiles : Profile
   {
      public MappingProfiles()
      {
         CreateMap<AppUser, UserDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => "********"))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => "********"));
      }
   }
}