using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class GetBlogByIdQueryHandler : IRequestHandler<GetBlogByIdQuery, Result<BlogResponseDto>>
{
	private readonly IBlogRepository _repository;
	private readonly IMapper _mapper;

	public GetBlogByIdQueryHandler(IBlogRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<BlogResponseDto>> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
	{
		var blog = await _repository.GetByIdWithDetailsAsync(request.Id);
		if (blog == null)
			return Result<BlogResponseDto>.FailureResult("Blog has not found.");

		var dto = _mapper.Map<BlogResponseDto>(blog);
		return Result<BlogResponseDto>.SuccessResult(dto);
	}
}