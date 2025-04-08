using DogusProject.Application.Common;
using DogusProject.Application.Features.Tags.Commands;
using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Tags.Handlers;

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Result<Guid>>
{
	private readonly ITagRepository _repository;

	public CreateTagCommandHandler(ITagRepository repository)
	{
		_repository = repository;
	}

	public async Task<Result<Guid>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
	{
		var tag = new Tag { Name = request.Tag.Name };
		await _repository.AddAsync(tag);
		await _repository.SaveChangesAsync();

		return Result<Guid>.SuccessResult(tag.Id, "Tag created successfully.");
	}
}