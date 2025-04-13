using AutoMapper;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Categories.Commands;
using DogusProject.Application.Features.Categories.Dtos;
using DogusProject.Application.Features.Comments.Commands;
using DogusProject.Application.Features.Comments.Dtos;
using DogusProject.Application.Features.Tags.Commands;
using DogusProject.Application.Features.Tags.Dtos;
using DogusProject.Domain.Entities;

namespace DogusProject.Application.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		// Blog
		CreateMap<Blog, BlogResponseDto>()
			.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
			.ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.BlogTags.Select(bt => bt.Tag.Name)))
			.ForMember(dest => dest.Author, opt => opt.Ignore());

		CreateMap<CreateBlogDto, Blog>()
			.ForMember(dest => dest.BlogTags, opt => opt.Ignore());

		CreateMap<UpdateBlogDto, Blog>()
			.ForMember(dest => dest.BlogTags, opt => opt.Ignore())
			.ForMember(dest => dest.Category, opt => opt.Ignore())
			.ForMember(dest => dest.UserId, opt => opt.Ignore());

		CreateMap<Blog, UpdateBlogDto>()
			.ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.BlogTags.Select(bt => bt.TagId)));

		CreateMap<Blog, BlogDetailDto>()
			.ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.BlogTags.Select(bt => bt.Tag)));

		// Category
		CreateMap<Category, CategoryDto>().ReverseMap();
		CreateMap<CreateCategoryDto, Category>();
		CreateMap<CreateCategoryCommand, Category>();
		CreateMap<UpdateCategoryCommand, Category>();
		CreateMap<Category, PopularCategoryDto>()
			.ForMember(dest => dest.BlogCount, opt => opt.MapFrom(src => src.Blogs.Count));

		// Tag
		CreateMap<CreateTagCommand, Tag>();
		CreateMap<UpdateTagCommand, Tag>();
		CreateMap<Tag, TagDto>().ReverseMap();
		CreateMap<Tag, PopularTagDto>()
			.ForMember(dest => dest.BlogCount, opt => opt.MapFrom(src => src.BlogTags.Count));

		// Comment
		CreateMap<CreateCommentCommand, Comment>();
		CreateMap<Comment, CommentDto>();
		CreateMap<Comment, CommentResponseDto>()
			.ForMember(dest => dest.BlogTitle, opt => opt.MapFrom(src => src.Blog.Title));
		CreateMap<CreateCommentDto, Comment>();
	}
}