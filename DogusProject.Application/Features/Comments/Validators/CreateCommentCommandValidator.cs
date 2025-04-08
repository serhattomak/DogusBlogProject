using DogusProject.Application.Features.Comments.Commands;
using FluentValidation;

namespace DogusProject.Application.Features.Comments.Validators;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
	public CreateCommentCommandValidator()
	{
		RuleFor(x => x.BlogId)
			.NotEmpty().WithMessage("You must pick a valid blog.");
		RuleFor(x => x.Content)
			.NotEmpty().WithMessage("Comment content cannot be null.")
			.MinimumLength(3).WithMessage("Comment must be at least 3 characters.");
	}
}