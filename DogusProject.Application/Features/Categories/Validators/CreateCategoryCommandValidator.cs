using DogusProject.Application.Features.Categories.Commands;
using FluentValidation;

namespace DogusProject.Application.Features.Categories.Validators;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
	public CreateCategoryCommandValidator()
	{
		RuleFor(x => x.Category.Name)
			.NotEmpty().WithMessage("Category name cannot be null.")
			.MinimumLength(2).WithMessage("Category name must be at least 2 characters.")
			.MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");
	}
}