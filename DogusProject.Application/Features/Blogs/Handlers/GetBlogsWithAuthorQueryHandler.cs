using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class GetBlogsWithAuthorQueryHandler : IRequestHandler<GetBlogsWithAuthorQuery, Result<List<BlogResponseDto>>>
{
	private readonly IBlogRepository _blogRepository;

	public GetBlogsWithAuthorQueryHandler(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;
	}

	public async Task<Result<List<BlogResponseDto>>> Handle(GetBlogsWithAuthorQuery request, CancellationToken cancellationToken)
	{
		var blogTuples = await _blogRepository.GetBlogsWithAuthorInfoByAuthorIdAsync(request.UserId);

		var dtos = blogTuples.Select(tuple => new BlogResponseDto
		{
			Id = tuple.Blog.Id,
			Title = tuple.Blog.Title,
			Content = tuple.Blog.Content,
			CreatedAt = tuple.Blog.CreatedAt,
			Status = tuple.Blog.Status,
			CategoryName = tuple.Blog.Category?.Name ?? string.Empty,
			ImageUrls = tuple.Blog.BlogImages.Select(i => i.ImageUrl).ToList(),
			AuthorFullName = tuple.AuthorFullName,
			AuthorAvatarUrl = tuple.AuthorAvatarUrl
		}).ToList();

		return Result<List<BlogResponseDto>>.SuccessResult(dtos);
	}
}