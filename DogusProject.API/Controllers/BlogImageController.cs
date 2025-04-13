using DogusProject.Application.Features.BlogImages.Commands;
using DogusProject.Application.Features.BlogImages.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogImageController : ControllerBase
	{
		private readonly IMediator _mediator;

		public BlogImageController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("upload/{blogId}")]
		[Authorize(Roles = "Author,Admin")]
		public async Task<IActionResult> UploadBlogImages([FromRoute] Guid blogId, [FromForm] List<IFormFile> images)
		{
			Console.WriteLine($"Gelen görsel sayısı: {images?.Count}");
			var command = new CreateBlogImageCommand
			{
				BlogId = blogId,
				Images = images
			};

			var result = await _mediator.Send(command);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("by-blog/{blogId}")]
		public async Task<IActionResult> GetByBlogId(Guid blogId)
		{
			var result = await _mediator.Send(new GetBlogImagesByBlogIdQuery(blogId));
			return result.Success ? Ok(result) : NotFound(result);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Author,Admin")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var result = await _mediator.Send(new DeleteBlogImageCommand(id));
			return result.Success ? Ok(result) : NotFound(result);
		}
	}
}
