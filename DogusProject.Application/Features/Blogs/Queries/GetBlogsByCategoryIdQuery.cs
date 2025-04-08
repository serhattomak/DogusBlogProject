using DogusProject.Application.Common;
using DogusProject.Application.Common.Pagination;
using DogusProject.Application.Features.Blogs.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Queries;

public class GetBlogsByCategoryIdQuery : PagedRequest, IRequest<Result<PagedResult<BlogResponseDto>>>
{
	public Guid CategoryId { get; set; }

	public GetBlogsByCategoryIdQuery(Guid categoryId, int page, int pageSize)
	{
		CategoryId = categoryId;
		Page = page;
		PageSize = pageSize;
	}
}