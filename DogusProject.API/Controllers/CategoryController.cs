using DogusProject.Application.Features.Categories.Commands;
using DogusProject.Application.Features.Categories.Dtos;
using DogusProject.Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CategoryController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		[Authorize(Roles = "Author,Admin")]
		public async Task<IActionResult> Create([FromBody] CreateCategoryDto request)
		{
			var result = await _mediator.Send(new CreateCategoryCommand(request));
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetAll()
		{
			var result = await _mediator.Send(new GetAllCategoriesQuery());
			return Ok(result);
		}

		[HttpGet("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetById(Guid id)
		{
			var result = await _mediator.Send(new GetCategoryByIdQuery(id));
			return result.Success ? Ok(result) : NotFound(result);
		}

		[HttpPut]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update([FromBody] CategoryDto dto)
		{
			var result = await _mediator.Send(new UpdateCategoryCommand(dto));
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var result = await _mediator.Send(new DeleteCategoryCommand(id));
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("popular")]
		[AllowAnonymous]
		public async Task<IActionResult> GetPopularCategories()
		{
			var result = await _mediator.Send(new GetPopularCategoriesQuery());
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
