using DogusProject.Application.Common;
using DogusProject.Application.Features.Tags.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Tags.Queries;

public class GetTagByIdQuery : IRequest<Result<TagDto>>
{
	public Guid Id { get; }

	public GetTagByIdQuery(Guid id)
	{
		Id = id;
	}
}