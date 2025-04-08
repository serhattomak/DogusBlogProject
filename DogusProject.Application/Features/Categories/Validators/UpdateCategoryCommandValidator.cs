using DogusProject.Application.Features.Categories.Commands;
using FluentValidation;

namespace DogusProject.Application.Features.Categories.Validators;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
	public UpdateCategoryCommandValidator()
	{
		RuleFor(x => x.Category.Id).NotEmpty().WithMessage("Category ID cannot be null.");
		RuleFor(x => x.Category.Name)
			.NotEmpty().WithMessage("Category name cannot be null.")
			.MinimumLength(2).WithMessage("Category name must be at least 2 characters.");
	}
}