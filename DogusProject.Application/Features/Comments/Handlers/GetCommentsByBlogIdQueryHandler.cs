using DogusProject.Application.Common;
using DogusProject.Application.Features.Comments.Dtos;
using DogusProject.Application.Features.Comments.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Comments.Handlers;

public class GetCommentsByBlogIdQueryHandler : IRequestHandler<GetCommentsByBlogIdQuery, Result<List<CommentResponseDto>>>
{
	private readonly ICommentRepository _commentRepository;

	public GetCommentsByBlogIdQueryHandler(ICommentRepository commentRepository)
	{
		_commentRepository = commentRepository;
	}

	public async Task<Result<List<CommentResponseDto>>> Handle(GetCommentsByBlogIdQuery request, CancellationToken cancellationToken)
	{
		var rawComments = await _commentRepository.GetCommentsWithAuthorsByBlogIdAsync(request.BlogId);

		var dtos = rawComments.Select(rc => new CommentResponseDto
		{
			Id = rc.Comment.Id,
			BlogId = rc.Comment.BlogId,
			UserId = rc.Comment.UserId,
			Content = rc.Comment.Content,
			CreatedAt = rc.Comment.CreatedAt,
			AuthorFullName = rc.AuthorFullName ?? "Anonim"
		}).ToList();

		return Result<List<CommentResponseDto>>.SuccessResult(dtos);
	}
}