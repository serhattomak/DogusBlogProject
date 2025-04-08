using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Commands;

public class CreateBlogCommand : IRequest<Result<Guid>>
{
	public CreateBlogDto Blog { get; set; }
	public Guid UserId { get; set; }

	public CreateBlogCommand(CreateBlogDto blog, Guid userId)
	{
		Blog = blog;
		UserId = userId;
	}
}