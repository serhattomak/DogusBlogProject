using DogusProject.Application.Common;
using DogusProject.Application.Features.Users.Dtos;
using Microsoft.AspNetCore.Http;

namespace DogusProject.Application.Interfaces;

public interface IUserService
{
	Task<Result<string>> UploadAvatarAsync(Guid userId, IFormFile file, CancellationToken cancellationToken);
	Task<Result<UserProfileDto>> GetProfileAsync(Guid userId, CancellationToken cancellationToken);
	Task<Result<string>> UpdateProfileAsync(UpdateProfileDto dto, CancellationToken cancellationToken);
}