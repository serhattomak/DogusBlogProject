using DogusProject.Application.Common;
using MediatR;

namespace DogusProject.Application.Features.Tags.Commands;

public class DeleteTagCommand : IRequest<Result>
{
	public Guid Id { get; }

	public DeleteTagCommand(Guid id)
	{
		Id = id;
	}
}