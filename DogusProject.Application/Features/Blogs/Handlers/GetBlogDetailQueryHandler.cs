using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using DogusProject.Application.Features.Categories.Dtos;
using DogusProject.Application.Features.Tags.Dtos;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class GetBlogDetailQueryHandler : IRequestHandler<GetBlogDetailQuery, Result<BlogDetailDto>>
{
	private readonly IBlogRepository _blogRepository;
	private readonly IBlogImageRepository _blogImageRepository;
	private readonly IMapper _mapper;

	public GetBlogDetailQueryHandler(IBlogRepository blogRepository, IMapper mapper, IBlogImageRepository blogImageRepository)
	{
		_blogRepository = blogRepository;
		_mapper = mapper;
		_blogImageRepository = blogImageRepository;
	}

	public async Task<Result<BlogDetailDto>> Handle(GetBlogDetailQuery request, CancellationToken cancellationToken)
	{
		var (blog, authorName) = await _blogRepository.GetBlogWithAuthorAsync(request.BlogId);
		if (blog == null)
			return Result<BlogDetailDto>.FailureResult("Blog bulunamadı.");

		var images = await _blogImageRepository.GetImagesByBlogIdAsync(blog.Id);

		var dto = new BlogDetailDto
		{
			Id = blog.Id,
			Title = blog.Title,
			Content = blog.Content,
			Category = new CategoryDto { Id = blog.Category.Id, Name = blog.Category.Name },
			Tags = blog.BlogTags.Select(t => new TagDto { Id = t.Tag.Id, Name = t.Tag.Name }).ToList(),
			ImageUrls = images.Select(i => i.ImageUrl).ToList(),
			UserId = blog.UserId,
			Author = authorName,
			CreatedAt = blog.CreatedAt,
			PublishedAt = blog.PublishedAt
		};

		return Result<BlogDetailDto>.SuccessResult(dto);
	}
}