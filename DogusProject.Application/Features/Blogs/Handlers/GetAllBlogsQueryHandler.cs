using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Common.Pagination;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class GetAllBlogsQueryHandler : IRequestHandler<GetAllBlogsQuery, Result<PagedResult<BlogResponseDto>>>
{
	private readonly IBlogRepository _repository;
	private readonly IMapper _mapper;

	public GetAllBlogsQueryHandler(IBlogRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<PagedResult<BlogResponseDto>>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
	{
		var blogs = await _repository.GetAllWithCategoryAsync();
		var paged = blogs
			.Skip((request.Page - 1) * request.PageSize)
			.Take(request.PageSize)
			.OrderByDescending(x => x.CreatedAt)
			.ToList();
		var mapped = _mapper.Map<List<BlogResponseDto>>(paged);
		var result = new PagedResult<BlogResponseDto>(mapped, blogs.Count, request.Page, request.PageSize);
		return Result<PagedResult<BlogResponseDto>>.SuccessResult(result);
	}
}