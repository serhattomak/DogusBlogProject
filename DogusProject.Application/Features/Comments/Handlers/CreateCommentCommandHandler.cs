using DogusProject.Application.Common;
using DogusProject.Application.Features.Comments.Commands;
using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Comments.Handlers;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<Guid>>
{
	private readonly ICommentRepository _repository;

	public CreateCommentCommandHandler(ICommentRepository repository)
	{
		_repository = repository;
	}

	public async Task<Result<Guid>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
	{
		var comment = new Comment
		{
			BlogId = request.BlogId,
			UserId = request.UserId,
			Content = request.Content,
			CreatedAt = DateTime.UtcNow
		};

		await _repository.AddAsync(comment);
		await _repository.SaveChangesAsync();

		return Result<Guid>.SuccessResult(comment.Id, "Comment added successfully.");
	}
}