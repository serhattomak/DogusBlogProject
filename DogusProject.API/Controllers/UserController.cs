using DogusProject.Application.Features.Users.Commands;
using DogusProject.Application.Features.Users.Dtos;
using DogusProject.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IMediator _mediator;

		public UserController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("upload-avatar")]
		[Authorize]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarRequest request, CancellationToken cancellationToken)
		{
			var command = new UploadAvatarCommand
			{
				UserId = request.UserId,
				File = request.File
			};

			var result = await _mediator.Send(command, cancellationToken);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("profile/{userId}")]
		[Authorize]
		public async Task<IActionResult> GetProfile(Guid userId)
		{
			var result = await _mediator.Send(new GetProfileQuery(userId));
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPut("edit-profile")]
		[Authorize]
		public async Task<IActionResult> EditProfile([FromBody] UpdateProfileDto dto, CancellationToken cancellationToken)
		{
			var command = new UpdateProfileCommand(dto);
			var result = await _mediator.Send(command, cancellationToken);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
