using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Tags.Dtos;
using DogusProject.Application.Features.Tags.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Tags.Handlers;

public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, Result<List<TagDto>>>
{
	private readonly ITagRepository _repository;
	private readonly IMapper _mapper;

	public GetAllTagsQueryHandler(ITagRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<List<TagDto>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
	{
		var tags = await _repository.GetAllAsync();
		var dto = _mapper.Map<List<TagDto>>(tags);
		return Result<List<TagDto>>.SuccessResult(dto);
	}
}