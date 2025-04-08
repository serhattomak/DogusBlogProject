using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Commands;

public class UpdateBlogCommand : IRequest<Result>
{
	public UpdateBlogDto Blog { get; set; }
	public Guid UserId { get; set; }

	public UpdateBlogCommand(UpdateBlogDto blog, Guid userId)
	{
		Blog = blog;
		UserId = userId;
	}
}