using DogusProject.Application.Common;
using DogusProject.Application.Common.Pagination;
using DogusProject.Application.Features.Blogs.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Queries;

public class GetBlogsByTagIdQuery : PagedRequest, IRequest<Result<PagedResult<BlogResponseDto>>>
{
	public Guid TagId { get; set; }

	public GetBlogsByTagIdQuery(Guid tagId, int page, int pageSize)
	{
		TagId = tagId;
		Page = page;
		PageSize = pageSize;
	}
}