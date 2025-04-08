using DogusProject.Application.Common;
using DogusProject.Application.Features.Tags.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Tags.Commands;

public class CreateTagCommand : IRequest<Result<Guid>>
{
	public CreateTagDto Tag { get; set; }

	public CreateTagCommand(CreateTagDto tag)
	{
		Tag = tag;
	}
}