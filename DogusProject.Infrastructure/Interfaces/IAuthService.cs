using DogusProject.Application.Common;
using DogusProject.Application.DTOs;
using DogusProject.Infrastructure.Identity;

namespace DogusProject.Infrastructure.Interfaces;

public interface IAuthService
{
	Task<Result> RegisterAsync(RegisterRequestDto registerDto);
	Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto loginDto);
	string CreateToken(AppUser user, IList<string> roles);
}