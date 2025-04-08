using AutoMapper;
using DogusProject.Application.DTOs;
using DogusProject.Infrastructure.Identity;

namespace DogusProject.Infrastructure.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<RegisterRequestDto, AppUser>()
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
	}
}