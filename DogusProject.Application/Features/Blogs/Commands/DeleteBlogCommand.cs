using DogusProject.Application.Common;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Commands;

public class DeleteBlogCommand : IRequest<Result>
{
	public Guid BlogId { get; set; }
	public Guid UserId { get; set; }

	public DeleteBlogCommand(Guid blogId, Guid userId)
	{
		BlogId = blogId;
		UserId = userId;
	}
}