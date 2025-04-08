using DogusProject.Application.Common;
using DogusProject.Application.Common.Pagination;
using DogusProject.Application.Features.Blogs.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Queries;

public class GetBlogsByAuthorIdQuery : PagedRequest, IRequest<Result<PagedResult<BlogResponseDto>>>
{
	public Guid UserId { get; set; }

	public GetBlogsByAuthorIdQuery(Guid userId, int page, int pageSize)
	{
		UserId = userId;
		Page = page;
		PageSize = pageSize;
	}
}