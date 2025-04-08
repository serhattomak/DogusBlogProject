using DogusProject.Application.Common;
using MediatR;

namespace DogusProject.Application.Features.Comments.Commands;

public class CreateCommentCommand : IRequest<Result<Guid>>
{
	public Guid BlogId { get; set; }
	public string Content { get; set; } = null!;
	public Guid UserId { get; set; }

	public CreateCommentCommand(Guid blogId, string content, Guid userId)
	{
		BlogId = blogId;
		Content = content;
		UserId = userId;
	}
}