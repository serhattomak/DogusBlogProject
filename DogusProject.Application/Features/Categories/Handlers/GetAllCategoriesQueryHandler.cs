using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Categories.Dtos;
using DogusProject.Application.Features.Categories.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Categories.Handlers;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Result<List<CategoryDto>>>
{
	private readonly ICategoryRepository _repository;
	private readonly IMapper _mapper;

	public GetAllCategoriesQueryHandler(ICategoryRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
	{
		var categories = await _repository.GetAllAsync();
		var dto = _mapper.Map<List<CategoryDto>>(categories.OrderByDescending(x => x.CreatedAt));
		return Result<List<CategoryDto>>.SuccessResult(dto);
	}
}