using DogusProject.Application.Common;
using MediatR;

namespace DogusProject.Application.Features.BlogImages.Queries;

public class GetBlogImagesByBlogIdQuery : IRequest<Result<List<BlogImageDto>>>
{
	public Guid BlogId { get; set; }

	public GetBlogImagesByBlogIdQuery(Guid blogId)
	{
		BlogId = blogId;
	}
}