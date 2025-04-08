using DogusProject.Application.Features.Comments.Commands;
using DogusProject.Application.Features.Comments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogusProject.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CommentController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Create(Guid blogId, [FromBody] string content)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized();

			var command = new CreateCommentCommand(blogId, content, Guid.Parse(userId));
			var result = await _mediator.Send(command);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("{blogId}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetByBlogId(Guid blogId)
		{
			var result = await _mediator.Send(new GetCommentsByBlogIdQuery(blogId));
			return Ok(result);
		}
	}
}
