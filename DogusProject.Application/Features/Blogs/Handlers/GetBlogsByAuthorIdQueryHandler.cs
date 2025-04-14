using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Common.Pagination;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class GetBlogsByAuthorIdQueryHandler : IRequestHandler<GetBlogsByAuthorIdQuery, Result<PagedResult<BlogResponseDto>>>
{
	private readonly IBlogRepository _blogRepository;
	private readonly IBlogImageRepository _blogImageRepository;
	private readonly IMapper _mapper;

	public GetBlogsByAuthorIdQueryHandler(IBlogRepository blogRepository, IMapper mapper, IBlogImageRepository blogImageRepository)
	{
		_blogRepository = blogRepository;
		_mapper = mapper;
		_blogImageRepository = blogImageRepository;
	}

	public async Task<Result<PagedResult<BlogResponseDto>>> Handle(GetBlogsByAuthorIdQuery request, CancellationToken cancellationToken)
	{
		var blogs = await _blogRepository.GetBlogsByAuthorIdAsync(request.UserId);
		if (blogs == null)
			return Result<PagedResult<BlogResponseDto>>.FailureResult("Blog bulunamadı.");

		var paged = blogs
			.OrderByDescending(x => x.CreatedAt)
			.Skip((request.Page - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToList();

		var blogDtos = new List<BlogResponseDto>();

		foreach (var blog in paged)
		{
			var images = await _blogImageRepository.GetImagesByBlogIdAsync(blog.Id);
			var dto = new BlogResponseDto
			{
				Id = blog.Id,
				Title = blog.Title,
				Content = blog.Content,
				CreatedAt = blog.CreatedAt,
				CategoryId = blog.CategoryId,
				CategoryName = blog.Category?.Name ?? "Bilinmiyor",
				AuthorFullName = "",
				ImageUrls = images.Select(i => i.ImageUrl).ToList(),
				Status = blog.Status
			};
			blogDtos.Add(dto);
		}

		return Result<PagedResult<BlogResponseDto>>.SuccessResult(new PagedResult<BlogResponseDto>(
			blogDtos, blogs.Count, request.Page, request.PageSize));
	}
}