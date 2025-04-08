using DogusProject.Application.Common;
using DogusProject.Application.Features.Tags.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Tags.Commands;

public class UpdateTagCommand : IRequest<Result>
{
	public TagDto Tag { get; set; }

	public UpdateTagCommand(TagDto tag)
	{
		Tag = tag;
	}
}