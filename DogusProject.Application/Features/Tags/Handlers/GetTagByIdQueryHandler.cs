using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Tags.Dtos;
using DogusProject.Application.Features.Tags.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Tags.Handlers;

public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, Result<TagDto>>
{
	private readonly ITagRepository _repository;
	private readonly IMapper _mapper;

	public GetTagByIdQueryHandler(ITagRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<TagDto>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
	{
		var tag = await _repository.GetByIdAsync(request.Id);
		if (tag == null)
			return Result<TagDto>.FailureResult("Tag has not found.");

		var dto = _mapper.Map<TagDto>(tag);
		return Result<TagDto>.SuccessResult(dto);
	}
}