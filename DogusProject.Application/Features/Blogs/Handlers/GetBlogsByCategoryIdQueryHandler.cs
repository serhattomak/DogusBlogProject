using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Common.Pagination;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class GetBlogsByCategoryIdQueryHandler : IRequestHandler<GetBlogsByCategoryIdQuery, Result<PagedResult<BlogResponseDto>>>
{
	private readonly IBlogRepository _blogRepository;
	private readonly IBlogImageRepository _blogImageRepository;
	private readonly IMapper _mapper;

	public GetBlogsByCategoryIdQueryHandler(IBlogRepository blogRepository, IMapper mapper, IBlogImageRepository blogImageRepository)
	{
		_blogRepository = blogRepository;
		_mapper = mapper;
		_blogImageRepository = blogImageRepository;
	}

	public async Task<Result<PagedResult<BlogResponseDto>>> Handle(GetBlogsByCategoryIdQuery request, CancellationToken cancellationToken)
	{
		var blogs = await _blogRepository.GetBlogsByCategoryIdAsync(request.CategoryId);

		if (blogs == null || !blogs.Any())
			return Result<PagedResult<BlogResponseDto>>.FailureResult("Blog not found.");

		var paged = blogs
			.OrderByDescending(x => x.CreatedAt)
			.Skip((request.Page - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToList();

		var mapped = _mapper.Map<List<BlogResponseDto>>(paged);

		foreach (var blogDto in mapped)
		{
			var images = await _blogImageRepository.GetImagesByBlogIdAsync(blogDto.Id);
			blogDto.ImageUrls = images.Select(i => i.ImageUrl).ToList();
		}

		var result = new PagedResult<BlogResponseDto>(mapped, blogs.Count, request.Page, request.PageSize);
		return Result<PagedResult<BlogResponseDto>>.SuccessResult(result);
	}
}