using AutoMapper;
using DogusProject.Web.Models.Blog.DTOs;
using DogusProject.Web.Models.Blog.ViewModels;

namespace DogusProject.Web.Mapping;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<UpdateBlogViewModel, UpdateBlogDto>()
			.ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.SelectedTagIds));
		CreateMap<UpdateBlogDto, UpdateBlogViewModel>()
			.ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.TagIds));
	}
}