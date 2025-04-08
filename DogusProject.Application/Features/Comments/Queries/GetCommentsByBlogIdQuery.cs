using DogusProject.Application.Common;
using DogusProject.Application.Features.Comments.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Comments.Queries;

public class GetCommentsByBlogIdQuery : IRequest<Result<List<CommentDto>>>
{
	public Guid BlogId { get; }

	public GetCommentsByBlogIdQuery(Guid blogId)
	{
		BlogId = blogId;
	}
}