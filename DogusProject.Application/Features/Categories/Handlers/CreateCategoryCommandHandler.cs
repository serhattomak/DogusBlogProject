using DogusProject.Application.Common;
using DogusProject.Application.Features.Categories.Commands;
using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Categories.Handlers;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
	private readonly ICategoryRepository _repository;

	public CreateCategoryCommandHandler(ICategoryRepository repository)
	{
		_repository = repository;
	}

	public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = new Category { Name = request.Category.Name };
		await _repository.AddAsync(category);
		await _repository.SaveChangesAsync();

		return Result<Guid>.SuccessResult(category.Id, "Category created successfully.");
	}
}