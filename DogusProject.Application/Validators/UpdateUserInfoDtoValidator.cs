using DogusProject.Application.DTOs;
using FluentValidation;

namespace DogusProject.Application.Validators;

public class UpdateUserInfoDtoValidator : AbstractValidator<UpdateUserInfoDto>
{
	public UpdateUserInfoDtoValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty().WithMessage("User ID cannot be null.");
		RuleFor(x => x.UserName)
			.NotEmpty().WithMessage("Username cannot be null.")
			.MaximumLength(100).WithMessage("Username cannot exceed 100 characters.");
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email cannot be null.")
			.EmailAddress().WithMessage("Please enter a valid email address.");
	}
}