using DogusProject.Application.Features.Blogs.Commands;
using FluentValidation;

namespace DogusProject.Application.Features.Blogs.Validators;

public class CreateBlogCommandValidator : AbstractValidator<CreateBlogCommand>
{
	public CreateBlogCommandValidator()
	{
		RuleFor(x => x.Blog.Title)
			.NotEmpty().WithMessage("Title is required.")
			.MaximumLength(150).WithMessage("Title must not exceed 150 characters.");
		RuleFor(x => x.Blog.Content)
			.NotEmpty().WithMessage("Content is required.")
			.MinimumLength(10).WithMessage("Content length must be at least 10 characters.");
		RuleFor(x => x.Blog.CategoryId)
			.NotEmpty().WithMessage("Category is required.");
	}
}