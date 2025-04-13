using DogusProject.Application.Common;
using MediatR;

namespace DogusProject.Application.Features.BlogImages.Commands;

public class DeleteBlogImageCommand : IRequest<Result<string>>
{
	public Guid ImageId { get; set; }

	public DeleteBlogImageCommand(Guid imageId)
	{
		ImageId = imageId;
	}
}