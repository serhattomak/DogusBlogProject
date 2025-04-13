using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Tags.Dtos;
using DogusProject.Application.Features.Tags.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Tags.Handlers;

public class GetPopularTagsQueryHandler : IRequestHandler<GetPopularTagsQuery, Result<List<PopularTagDto>>>
{
	private readonly ITagRepository _repository;
	private readonly IMapper _mapper;

	public GetPopularTagsQueryHandler(ITagRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<List<PopularTagDto>>> Handle(GetPopularTagsQuery request, CancellationToken cancellationToken)
	{
		var tags = await _repository.GetAllWithBlogCountAsync();
		var dto = _mapper.Map<List<PopularTagDto>>(tags).OrderByDescending(t => t.BlogCount).ToList();

		return Result<List<PopularTagDto>>.SuccessResult(dto);
	}
}