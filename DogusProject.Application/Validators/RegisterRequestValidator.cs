﻿using DogusProject.Application.DTOs;
using FluentValidation;

namespace DogusProject.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
	public RegisterRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email cannot be null.")
			.EmailAddress().WithMessage("Please enter a valid email address");
		RuleFor(x => x.UserName)
			.NotEmpty().WithMessage("Username cannot be null.")
			.MaximumLength(100).WithMessage("Username cannot exceed 100 characters.");
		RuleFor(x => x.Password)
			.MinimumLength(6).WithMessage("Password must be at least 6 characters");
	}
}