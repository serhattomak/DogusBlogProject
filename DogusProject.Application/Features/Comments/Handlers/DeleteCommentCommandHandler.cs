using DogusProject.Application.Common;
using DogusProject.Application.Features.Comments.Commands;
using DogusProject.Domain.Exceptions;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Comments.Handlers;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result<Guid>>
{
	private readonly ICommentRepository _commentRepository;

	public DeleteCommentCommandHandler(ICommentRepository commentRepository)
	{
		_commentRepository = commentRepository;
	}

	public async Task<Result<Guid>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
	{
		var comment = await _commentRepository.GetByIdAsync(request.Id);
		if (comment == null)
			throw new NotFoundException("Comment", request.Id);

		_commentRepository.Delete(comment);
		return Result<Guid>.SuccessResult(comment.Id);
	}
}