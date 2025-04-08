using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Comments.Commands;
using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Comments.Handlers;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<Guid>>
{
	private readonly ICommentRepository _repository;
	private readonly IMapper _mapper;

	public CreateCommentCommandHandler(ICommentRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<Guid>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
	{
		var comment = _mapper.Map<Comment>(request);
		comment.CreatedAt = DateTime.UtcNow;

		await _repository.AddAsync(comment);
		await _repository.SaveChangesAsync();

		return Result<Guid>.SuccessResult(comment.Id, "Comment added successfully.");
	}
}