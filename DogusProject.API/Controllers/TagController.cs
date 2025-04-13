using DogusProject.Application.Features.Tags.Commands;
using DogusProject.Application.Features.Tags.Dtos;
using DogusProject.Application.Features.Tags.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TagController : ControllerBase
	{
		private readonly IMediator _mediator;

		public TagController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([FromBody] CreateTagDto tag)
		{
			var result = await _mediator.Send(new CreateTagCommand(tag));
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetAll()
		{
			var result = await _mediator.Send(new GetAllTagsQuery());
			return Ok(result);
		}

		[HttpGet("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetById(Guid id)
		{
			var result = await _mediator.Send(new GetTagByIdQuery(id));
			return result.Success ? Ok(result) : NotFound(result);
		}

		[HttpPut]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update([FromBody] TagDto tag)
		{
			var result = await _mediator.Send(new UpdateTagCommand(tag));
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var result = await _mediator.Send(new DeleteTagCommand(id));
			return result.Success ? Ok(result) : BadRequest(result);
		}
		[HttpGet("popular")]
		[AllowAnonymous]
		public async Task<IActionResult> GetPopularTags()
		{
			var result = await _mediator.Send(new GetPopularTagsQuery());
			return result.Success ? Ok(result) : NotFound(result);
		}
	}
}
