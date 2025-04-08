using AutoMapper;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Categories.Dtos;
using DogusProject.Application.Features.Comments.Commands;
using DogusProject.Application.Features.Comments.Dtos;
using DogusProject.Application.Features.Tags.Dtos;
using DogusProject.Domain.Entities;

namespace DogusProject.Application.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Blog, BlogResponseDto>()
			.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
			.ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.BlogTags.Select(bt => bt.Tag.Name)))
			.ForMember(dest => dest.Author, opt => opt.Ignore());

		CreateMap<CreateBlogDto, Blog>()
			.ForMember(dest => dest.BlogTags, opt => opt.Ignore());

		CreateMap<UpdateBlogDto, Blog>()
			.ForMember(dest => dest.BlogTags, opt => opt.Ignore());

		CreateMap<Category, CategoryDto>().ReverseMap();
		CreateMap<CreateCategoryDto, Category>();

		CreateMap<Tag, TagDto>().ReverseMap();


		CreateMap<CreateCommentCommand, Comment>();
		CreateMap<Comment, CommentDto>();
	}
}