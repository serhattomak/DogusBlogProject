using DogusProject.Application.Features.Tags.Commands;
using FluentValidation;

namespace DogusProject.Application.Features.Tags.Validators;

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
	public UpdateTagCommandValidator()
	{
		RuleFor(x => x.Tag.Id).NotEmpty().WithMessage("Tag ID cannot be null.");
		RuleFor(x => x.Tag.Name)
			.NotEmpty().WithMessage("Tag name cannot be null.")
			.MinimumLength(2).WithMessage("Tag name must be at least 2 characters.");
	}
}