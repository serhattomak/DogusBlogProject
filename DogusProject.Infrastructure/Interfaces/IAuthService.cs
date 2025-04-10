using DogusProject.Application.Common;
using DogusProject.Application.Common.Pagination;
using DogusProject.Application.DTOs;
using DogusProject.Infrastructure.Identity;

namespace DogusProject.Infrastructure.Interfaces;

public interface IAuthService
{
	Task<Result> RegisterAsync(RegisterRequestDto registerDto);
	Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto loginDto);
	Task<Result<string>> UpdateUserInfoAsync(UpdateUserInfoDto dto);
	Task<Result<string>> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
	Task<Result<string>> CreateRoleAsync(string roleName);
	Task<Result<string>> AssignRoleToUserAsync(AssignRoleRequestDto dto);
	Task<Result<List<string>>> GetUserRolesAsync(Guid userId);
	Task<Result<string>> RemoveUserFromRoleAsync(RemoveUserRoleRequestDto dto);
	Task<Result<List<string>>> GetAllRolesAsync();
	Task<Result<UserInfoDto>> GetUserByIdAsync(Guid userId);
	Task<Result<PagedResult<UserListItemDto>>> GetUsersAsync(UserListRequestDto request);
	Task<Result<UserInfoDto>> GetUserByEmail(string email);
	string CreateToken(AppUser user, IList<string> roles);
}