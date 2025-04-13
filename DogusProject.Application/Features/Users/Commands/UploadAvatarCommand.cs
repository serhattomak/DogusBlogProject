using DogusProject.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DogusProject.Application.Features.Users.Commands;

public class UploadAvatarCommand : IRequest<Result<string>>
{
	public Guid UserId { get; set; }
	public IFormFile File { get; set; } = null!;
}