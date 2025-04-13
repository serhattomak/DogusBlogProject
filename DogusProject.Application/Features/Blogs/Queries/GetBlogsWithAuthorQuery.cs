using DogusProject.Application.Common;
using DogusProject.Application.Common.Pagination;
using DogusProject.Application.Features.Blogs.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Queries;

public class GetBlogsWithAuthorQuery : PagedRequest, IRequest<Result<List<BlogResponseDto>>>
{
	public Guid UserId { get; set; }
	public GetBlogsWithAuthorQuery(Guid userId)
	{
		UserId = userId;
	}
}