using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Categories.Dtos;
using DogusProject.Application.Features.Categories.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Categories.Handlers;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
	private readonly ICategoryRepository _repository;
	private readonly IMapper _mapper;

	public GetCategoryByIdQueryHandler(ICategoryRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
	{
		var category = await _repository.GetByIdAsync(request.Id);
		if (category == null)
			return Result<CategoryDto>.FailureResult("Category has not found.");

		var dto = _mapper.Map<CategoryDto>(category);
		return Result<CategoryDto>.SuccessResult(dto);
	}
}