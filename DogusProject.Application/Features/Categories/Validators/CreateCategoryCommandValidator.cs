using DogusProject.Application.Features.Categories.Commands;
using FluentValidation;

namespace DogusProject.Application.Features.Categories.Validators;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
	public CreateCategoryCommandValidator()
	{
		RuleFor(x => x.Category.Name).NotEmpty().MinimumLength(2);
	}
}