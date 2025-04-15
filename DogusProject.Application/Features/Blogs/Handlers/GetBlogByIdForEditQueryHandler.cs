using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class GetBlogByIdForEditQueryHandler : IRequestHandler<GetBlogByIdForEditQuery, Result<UpdateBlogDto>>
{
	private readonly IBlogRepository _repository;
	private readonly IMapper _mapper;

	public GetBlogByIdForEditQueryHandler(IBlogRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<UpdateBlogDto>> Handle(GetBlogByIdForEditQuery request, CancellationToken cancellationToken)
	{
		var blog = await _repository.GetByIdWithCategoryAndTagsAsync(request.BlogId);
		if (blog is null)
			return Result<UpdateBlogDto>.FailureResult("Blog bulunamadı.");

		var dto = _mapper.Map<UpdateBlogDto>(blog);
		return Result<UpdateBlogDto>.SuccessResult(dto);
	}
}