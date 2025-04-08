using DogusProject.Application.Common;
using DogusProject.Application.Features.Categories.Commands;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Categories.Handlers;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
{
	private readonly ICategoryRepository _repository;

	public UpdateCategoryCommandHandler(ICategoryRepository repository)
	{
		_repository = repository;
	}

	public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await _repository.GetByIdAsync(request.Category.Id);
		if (category == null)
			return Result.FailureResult("Category has not found.");

		category.Name = request.Category.Name;
		_repository.Update(category);
		await _repository.SaveChangesAsync();

		return Result.SuccessResult("Category updated successfully.");
	}
}