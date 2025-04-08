using DogusProject.Application.Common;
using MediatR;

namespace DogusProject.Application.Features.Comments.Commands;

public class DeleteCommentCommand : IRequest<Result<Guid>>
{
	public Guid Id { get; set; }
}