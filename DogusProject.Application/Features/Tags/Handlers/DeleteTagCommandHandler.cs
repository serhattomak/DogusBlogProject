using DogusProject.Application.Common;
using DogusProject.Application.Features.Tags.Commands;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Tags.Handlers;

public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, Result>
{
	private readonly ITagRepository _repository;

	public DeleteTagCommandHandler(ITagRepository repository)
	{
		_repository = repository;
	}

	public async Task<Result> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
	{
		var tag = await _repository.GetByIdAsync(request.Id);
		if (tag == null)
			return Result.FailureResult("Tag has not found.");

		_repository.Delete(tag);
		await _repository.SaveChangesAsync();

		return Result.SuccessResult("Tag deleted successfully.");
	}
}