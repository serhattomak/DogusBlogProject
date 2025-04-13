using AutoMapper;
using DogusProject.Application.DTOs;
using DogusProject.Application.Features.Users.Dtos;
using DogusProject.Infrastructure.Identity;

namespace DogusProject.Infrastructure.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<RegisterRequestDto, AppUser>()
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

		CreateMap<AppUser, UserProfileDto>();
		CreateMap<UpdateProfileDto, AppUser>()
			.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
			.ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
			.ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
			.ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Website));
	}
}