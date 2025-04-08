using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Queries;

public class GetBlogByIdQuery : IRequest<Result<BlogResponseDto>>
{
	public Guid Id { get; }

	public GetBlogByIdQuery(Guid id)
	{
		Id = id;
	}
}