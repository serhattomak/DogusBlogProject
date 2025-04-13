using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using DogusProject.Domain.Common;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class GetAllBlogsWithAuthorQueryHandler : IRequestHandler<GetAllBlogsWithAuthorQuery, PagedResult<BlogResponseDto>>
{
	private readonly IBlogRepository _repository;

	public GetAllBlogsWithAuthorQueryHandler(IBlogRepository repository)
	{
		_repository = repository;
	}

	public async Task<PagedResult<BlogResponseDto>> Handle(GetAllBlogsWithAuthorQuery request, CancellationToken cancellationToken)
	{
		var blogTuples = await _repository.GetAllBlogsWithAuthorInfoAsync(request.Page, request.PageSize);

		var dtos = blogTuples.Items.Select(tuple => new BlogResponseDto
		{
			Id = tuple.Blog.Id,
			Title = tuple.Blog.Title,
			Content = tuple.Blog.Content,
			CreatedAt = tuple.Blog.CreatedAt,
			AuthorFullName = tuple.AuthorFullName ?? "Bilinmiyor"
		}).ToList();

		return new PagedResult<BlogResponseDto>
		{
			Items = dtos,
			CurrentPage = blogTuples.CurrentPage,
			TotalPages = blogTuples.TotalPages
		};
	}
}