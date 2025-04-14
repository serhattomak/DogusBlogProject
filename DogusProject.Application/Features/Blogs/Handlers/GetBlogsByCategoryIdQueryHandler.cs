using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using DogusProject.Domain.Common;
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
		var blogTuples = await _blogRepository.GetAllBlogsWithAuthorInfoAsync(request.Page, request.PageSize);
		var filteredBlogs = blogTuples.Items.Where(x => x.Blog.CategoryId == request.CategoryId).ToList();

		if (!filteredBlogs.Any())
			return Result<PagedResult<BlogResponseDto>>.FailureResult("No blogs found for the specified category.");

		var blogIds = filteredBlogs.Select(x => x.Blog.Id).ToList();
		var allImages = await _blogImageRepository.GetImageUrlsByBlogIdsAsync(blogIds);
		var imageDict = allImages
			.GroupBy(img => img.BlogId)
			.ToDictionary(g => g.Key, g => g.Select(x => x.ImageUrl).ToList());

		var dtos = filteredBlogs.Select(tuple => new BlogResponseDto
		{
			Id = tuple.Blog.Id,
			Title = tuple.Blog.Title,
			Content = tuple.Blog.Content,
			CreatedAt = tuple.Blog.CreatedAt,
			AuthorFullName = tuple.AuthorFullName ?? "Unknown",
			AuthorAvatarUrl = tuple.AuthorAvatarUrl,
			ImageUrls = imageDict.TryGetValue(tuple.Blog.Id, out var images) ? images : new()
		}).ToList();

		return Result<PagedResult<BlogResponseDto>>.SuccessResult(new PagedResult<BlogResponseDto>
		{
			Items = dtos,
			CurrentPage = request.Page,
			PageSize = request.PageSize,
			TotalCount = filteredBlogs.Count
		});
	}
}