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
	private readonly IMapper _mapper;

	public GetBlogsByAuthorIdQueryHandler(IBlogRepository blogRepository, IMapper mapper)
	{
		_blogRepository = blogRepository;
		_mapper = mapper;
	}

	public async Task<Result<PagedResult<BlogResponseDto>>> Handle(GetBlogsByAuthorIdQuery request, CancellationToken cancellationToken)
	{
		var blogs = await _blogRepository.GetBlogsByAuthorIdAsync(request.UserId);
		var paged = blogs
			.Skip((request.Page - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToList();
		var mapped = _mapper.Map<List<BlogResponseDto>>(blogs);
		return Result<PagedResult<BlogResponseDto>>.SuccessResult(new PagedResult<BlogResponseDto>(mapped, blogs.Count, request.Page, request.PageSize));
	}
}