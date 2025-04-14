using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using DogusProject.Domain.Common;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class GetAllBlogsWithAuthorQueryHandler : IRequestHandler<GetAllBlogsWithAuthorQuery, PagedResult<BlogResponseDto>>
{
	private readonly IBlogRepository _repository;
	private readonly IBlogImageRepository _blogImageRepository;

	public GetAllBlogsWithAuthorQueryHandler(IBlogRepository repository, IBlogImageRepository blogImageRepository)
	{
		_repository = repository;
		_blogImageRepository = blogImageRepository;
	}

	public async Task<PagedResult<BlogResponseDto>> Handle(GetAllBlogsWithAuthorQuery request, CancellationToken cancellationToken)
	{
		var blogTuples = await _repository.GetAllBlogsWithAuthorInfoAsync(request.Page, request.PageSize);
		var blogIds = blogTuples.Items.Select(x => x.Blog.Id).ToList();

		var allImages = await _blogImageRepository.GetImageUrlsByBlogIdsAsync(blogIds);
		var imageDict = allImages
			.GroupBy(img => img.BlogId)
			.ToDictionary(g => g.Key, g => g.Select(x => x.ImageUrl).ToList());

		var dtos = blogTuples.Items.Select(tuple => new BlogResponseDto
		{
			Id = tuple.Blog.Id,
			Title = tuple.Blog.Title,
			Content = tuple.Blog.Content,
			CreatedAt = tuple.Blog.CreatedAt,
			AuthorFullName = tuple.AuthorFullName ?? "Bilinmiyor",
			ImageUrls = imageDict.TryGetValue(tuple.Blog.Id, out var images) ? images : new()
		}).ToList();

		return new PagedResult<BlogResponseDto>
		{
			Items = dtos,
			CurrentPage = blogTuples.CurrentPage,
			PageSize = request.PageSize,
			TotalCount = blogTuples.TotalCount
		};
	}
}