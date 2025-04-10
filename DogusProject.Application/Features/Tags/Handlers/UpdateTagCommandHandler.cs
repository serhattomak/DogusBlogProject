using DogusProject.Application.Common;
using DogusProject.Application.Features.Tags.Commands;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Tags.Handlers;

public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, Result>
{
	private readonly ITagRepository _repository;

	public UpdateTagCommandHandler(ITagRepository repository)
	{
		_repository = repository;
	}

	public async Task<Result> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
	{
		var tag = await _repository.GetByIdAsync(request.Tag.Id);
		if (tag == null)
			return Result.FailureResult("Tag has not found.");

		tag.Name = request.Tag.Name;
		tag.UpdatedAt = DateTime.UtcNow;

		_repository.Update(tag);
		await _repository.SaveChangesAsync();

		return Result.SuccessResult("Tag updated successfully.");
	}
}