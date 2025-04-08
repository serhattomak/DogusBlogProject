using DogusProject.Application.Features.Tags.Commands;
using FluentValidation;

namespace DogusProject.Application.Features.Tags.Validators;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
	public CreateTagCommandValidator()
	{
		RuleFor(x => x.Tag.Name)
			.NotEmpty().WithMessage("Tag name cannot be null.")
			.MinimumLength(2).WithMessage("Tag name must be at least 2 characters.");
	}
}