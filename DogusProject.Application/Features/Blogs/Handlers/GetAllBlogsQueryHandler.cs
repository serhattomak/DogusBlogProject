using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class GetAllBlogsQueryHandler : IRequestHandler<GetAllBlogsQuery, Result<List<BlogResponseDto>>>
{
	private readonly IBlogRepository _repository;
	private readonly IMapper _mapper;

	public GetAllBlogsQueryHandler(IBlogRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<List<BlogResponseDto>>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
	{
		var blogs = await _repository.GetAllWithCategoryAsync();
		var dto = _mapper.Map<List<BlogResponseDto>>(blogs);
		return Result<List<BlogResponseDto>>.SuccessResult(dto);
	}
}