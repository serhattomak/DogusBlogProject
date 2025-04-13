using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.Application.Features.Users.Dtos;

public class UploadAvatarRequest
{
	[FromForm]
	public Guid UserId { get; set; }

	[FromForm]
	public IFormFile File { get; set; } = null!;
}