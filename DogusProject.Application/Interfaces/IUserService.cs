using DogusProject.Application.Common;
using Microsoft.AspNetCore.Http;

namespace DogusProject.Application.Interfaces;

public interface IUserService
{
	Task<Result<string>> UploadAvatarAsync(Guid userId, IFormFile file, CancellationToken cancellationToken);
}