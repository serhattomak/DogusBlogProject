using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Commands;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogusProject.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogController : ControllerBase
	{
		private readonly IMediator _mediator;

		public BlogController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("all")]
		[AllowAnonymous]
		public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
		{
			var result = await _mediator.Send(new GetAllBlogsQuery { Page = page, PageSize = pageSize });
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetById(Guid id)
		{
			var result = await _mediator.Send(new GetBlogByIdQuery(id));
			return result.Success ? Ok(result) : NotFound(result);
		}

		[HttpGet("by-category/{categoryId}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetByCategory(Guid categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
		{
			var result = await _mediator.Send(new GetBlogsByCategoryIdQuery(categoryId, page, pageSize));
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("by-tag/{tagId}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetByTag(Guid tagId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
		{
			var result = await _mediator.Send(new GetBlogsByTagIdQuery(tagId, page, pageSize));
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("by-author/{userId}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetByAuthor(Guid userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
		{
			var result = await _mediator.Send(new GetBlogsByAuthorIdQuery(userId, page, pageSize));
			if (!result.Success || result.Data == null)
			{
				return Ok(Result<List<BlogResponseDto>>.SuccessResult(new List<BlogResponseDto>()));
			}

			return Ok(result);
		}

		[HttpGet("detail/{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetDetail(Guid id)
		{
			var result = await _mediator.Send(new GetBlogDetailQuery(id));
			return result.Success ? Ok(result) : NotFound(result);
		}

		[HttpPost("create")]
		[Authorize(Roles = "Author, Admin")]
		public async Task<IActionResult> Create([FromBody] CreateBlogDto request)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized(Result.FailureResult("User not authorized."));

			var command = new CreateBlogCommand(request, Guid.Parse(userId));
			var result = await _mediator.Send(command);

			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPut]
		[Authorize(Roles = "Author,Admin")]
		public async Task<IActionResult> Update([FromBody] UpdateBlogDto dto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized(Result.FailureResult("User not authorized."));

			var command = new UpdateBlogCommand(dto, Guid.Parse(userId));
			var result = await _mediator.Send(command);

			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Author,Admin")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
				return Unauthorized(Result.FailureResult("User not authorized."));

			var command = new DeleteBlogCommand(id, Guid.Parse(userId));
			var result = await _mediator.Send(command);

			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
