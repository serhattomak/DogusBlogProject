using DogusProject.Application.Features.Blogs.Commands;
using FluentValidation;

namespace DogusProject.Application.Features.Blogs.Validators;

public class UpdateBlogCommandValidator : AbstractValidator<UpdateBlogCommand>
{
	public UpdateBlogCommandValidator()
	{
		RuleFor(x => x.Blog.Id).NotEmpty().WithMessage("Blog ID cannot be null.");
		RuleFor(x => x.Blog.Title)
			.NotEmpty().WithMessage("Title is required.")
			.MinimumLength(3).WithMessage("Title must be at least 3 characters.")
			.MaximumLength(150).WithMessage("Title must not exceed 150 characters.");

		RuleFor(x => x.Blog.Content)
			.NotEmpty().WithMessage("Content is required.")
			.MinimumLength(10).WithMessage("Content must be at least 10 characters.");

		RuleFor(x => x.Blog.CategoryId)
			.NotEmpty().WithMessage("Category is required.");
	}
}