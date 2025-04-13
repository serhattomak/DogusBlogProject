using DogusProject.Application.Common;
using DogusProject.Application.Interfaces;
using DogusProject.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DogusProject.Infrastructure.Services;

public class UserService : IUserService
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IFileService _fileService;

	public UserService(UserManager<AppUser> userManager, IFileService fileService)
	{
		_userManager = userManager;
		_fileService = fileService;
	}

	public async Task<Result<string>> UploadAvatarAsync(Guid userId, IFormFile file, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user == null)
			return Result<string>.FailureResult("User not found");

		var (fullPath, relativePath) = await _fileService.SaveFileAsync(file, "avatars", cancellationToken);

		user.AvatarPath = fullPath;
		user.AvatarUrl = relativePath;

		var result = await _userManager.UpdateAsync(user);
		return result.Succeeded
			? Result<string>.SuccessResult("Avatar uploaded successfully.")
			: Result<string>.FailureResult("Avatar update failed.");
	}
}