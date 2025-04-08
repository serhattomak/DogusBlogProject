using DogusProject.Application.Common;
using DogusProject.Application.Features.Categories.Commands;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Categories.Handlers;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
{
	private readonly ICategoryRepository _repository;

	public DeleteCategoryCommandHandler(ICategoryRepository repository)
	{
		_repository = repository;
	}

	public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await _repository.GetByIdAsync(request.Id);
		if (category == null)
			return Result.FailureResult("Category has not found.");

		_repository.Delete(category);
		await _repository.SaveChangesAsync();

		return Result.SuccessResult("Category deleted successfully.");
	}
}