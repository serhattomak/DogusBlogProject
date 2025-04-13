using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Users.Dtos;
using DogusProject.Application.Interfaces;
using DogusProject.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DogusProject.Infrastructure.Services;

public class UserService : IUserService
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IFileService _fileService;
	private readonly IMapper _mapper;

	public UserService(UserManager<AppUser> userManager, IFileService fileService, IMapper mapper)
	{
		_userManager = userManager;
		_fileService = fileService;
		_mapper = mapper;
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

	public async Task<Result<UserProfileDto>> GetProfileAsync(Guid userId, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user == null)
			return Result<UserProfileDto>.FailureResult("User not found");

		var dto = new UserProfileDto
		{
			UserId = user.Id,
			FirstName = user.FirstName,
			LastName = user.LastName,
			Bio = user.Bio,
			Location = user.Location,
			Website = user.Website,
			AvatarUrl = user.AvatarUrl,
			Email = user.Email,
			UserName = user.UserName
		};

		return Result<UserProfileDto>.SuccessResult(dto);
	}

	public async Task<Result<string>> UpdateProfileAsync(UpdateProfileDto dto, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
		if (user == null)
			return Result<string>.FailureResult("User not found");

		user.FirstName = dto.FirstName;
		user.LastName = dto.LastName;
		user.Bio = dto.Bio;
		user.Location = dto.Location;
		user.Website = dto.Website;

		var result = await _userManager.UpdateAsync(user);
		if (!result.Succeeded)
			return Result<string>.FailureResult(result.Errors.Select(e => e.Description).ToList());

		return Result<string>.SuccessResult("Profile updated successfully.");
	}
}