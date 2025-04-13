using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Categories.Dtos;
using DogusProject.Application.Features.Categories.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Categories.Handlers;

public class GetPopularCategoriesQueryHandler : IRequestHandler<GetPopularCategoriesQuery, Result<List<PopularCategoryDto>>>
{
	private readonly ICategoryRepository _repository;
	private readonly IMapper _mapper;

	public GetPopularCategoriesQueryHandler(ICategoryRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<List<PopularCategoryDto>>> Handle(GetPopularCategoriesQuery request, CancellationToken cancellationToken)
	{
		var categories = await _repository.GetAllWithBlogCountAsync();
		var dto = _mapper.Map<List<PopularCategoryDto>>(categories).OrderByDescending(c => c.BlogCount).ToList();

		return Result<List<PopularCategoryDto>>.SuccessResult(dto);
	}
}