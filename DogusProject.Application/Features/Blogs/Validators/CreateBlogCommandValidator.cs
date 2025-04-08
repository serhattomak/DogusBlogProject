using DogusProject.Application.Features.Blogs.Commands;
using FluentValidation;

namespace DogusProject.Application.Features.Blogs.Validators;

public class CreateBlogCommandValidator : AbstractValidator<CreateBlogCommand>
{
	public CreateBlogCommandValidator()
	{
		RuleFor(x => x.Blog.Title).NotEmpty().MinimumLength(3);
		RuleFor(x => x.Blog.Content).NotEmpty().MinimumLength(10);
		RuleFor(x => x.Blog.CategoryId).NotEmpty();
	}
}