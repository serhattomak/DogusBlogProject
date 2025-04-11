using DogusProject.Application.Common;
using DogusProject.Application.Features.Comments.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Comments.Queries;

public class GetCommentsByUserIdQuery : IRequest<Result<List<CommentResponseDto>>>
{
	public Guid UserId { get; set; }

	public GetCommentsByUserIdQuery(Guid userId)
	{
		UserId = userId;
	}
}