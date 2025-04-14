using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Common.Pagination;
using DogusProject.Application.DTOs;
using DogusProject.Infrastructure.Identity;
using DogusProject.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DogusProject.Infrastructure.Services;

public class AuthService : IAuthService
{
	private readonly IConfiguration _configuration;
	private readonly UserManager<AppUser> _userManager;
	private readonly SignInManager<AppUser> _signInManager;
	private readonly RoleManager<AppRole> _roleManager;
	private readonly ILogger<AuthService> _logger;
	private readonly IMapper _mapper;

	public AuthService(IConfiguration configuration, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<AuthService> logger, IMapper mapper)
	{
		_configuration = configuration;
		_roleManager = roleManager;
		_signInManager = signInManager;
		_userManager = userManager;
		_logger = logger;
		_mapper = mapper;
	}

	public async Task<Result> RegisterAsync(RegisterRequestDto registerDto)
	{
		_logger.LogInformation("Attempting to register a new user...");
		var user = _mapper.Map<AppUser>(registerDto);
		var result = await _userManager.CreateAsync(user, registerDto.Password);
		if (!result.Succeeded)
		{
			_logger.LogError("User registration failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
			return Result.FailureResult(result.Errors.Select(e => e.Description).ToList());
		}
		await _userManager.AddToRoleAsync(user, "Author");
		await _signInManager.SignInAsync(user, isPersistent: false);
		var roles = await _userManager.GetRolesAsync(user);
		return Result.SuccessResult("User created successfully.");
	}

	public async Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto loginDto)
	{
		_logger.LogInformation("Attempting to log in...");

		var user = await _userManager.FindByEmailAsync(loginDto.Email);
		if (user == null)
			throw new KeyNotFoundException("User not found.");

		var roles = await _userManager.GetRolesAsync(user);
		if (roles == null || roles.Count == 0)
			throw new KeyNotFoundException("User has no roles.");

		var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
		if (!result.Succeeded)
		{
			_logger.LogError("Invalid login attempt.");
			throw new UnauthorizedAccessException("Invalid login attempt.");
		}

		var response = new AuthResponseDto
		{
			Token = CreateToken(user, roles),
			UserId = user.Id,
			UserName = user.UserName,
			Roles = roles.ToList()
		};

		_logger.LogInformation("User logged in successfully.");
		return Result<AuthResponseDto>.SuccessResult(response);
	}

	public async Task<Result<string>> UpdateUserInfoAsync(UpdateUserInfoDto dto)
	{
		if (dto.UserId == Guid.Empty.ToString())
			return Result<string>.FailureResult("UserId is missing.");
		var user = await _userManager.FindByIdAsync(dto.UserId);
		if (user == null)
			return Result<string>.FailureResult("User not found.");

		user.UserName = dto.UserName;
		user.Email = dto.Email;

		var result = await _userManager.UpdateAsync(user);
		return result.Succeeded
			? Result<string>.SuccessResult("User informations updated.")
			: Result<string>.FailureResult("User informations could not updated.");
	}

	public async Task<Result<string>> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user == null)
			return Result<string>.FailureResult("User not found.");

		var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
		return result.Succeeded
			? Result<string>.SuccessResult("Password changed.")
			: Result<string>.FailureResult("Password could not changed.");
	}

	public async Task<Result<string>> CreateRoleAsync(string roleName)
	{
		if (await _roleManager.RoleExistsAsync(roleName))
			return Result<string>.FailureResult("This role already exists.");

		var result = await _roleManager.CreateAsync(new AppRole { Name = roleName });
		return result.Succeeded
			? Result<string>.SuccessResult("Role created successfully.")
			: Result<string>.FailureResult(result.Errors.Select(e => e.Description).ToList());
	}

	public async Task<Result<string>> AssignRoleToUserAsync(AssignRoleRequestDto dto)
	{
		var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
		if (user == null)
			return Result<string>.FailureResult("User not found.");

		if (!await _roleManager.RoleExistsAsync(dto.RoleName))
			return Result<string>.FailureResult("Role does not exist.");

		var result = await _userManager.AddToRoleAsync(user, dto.RoleName);
		return result.Succeeded
			? Result<string>.SuccessResult("Role assigned to the user successfully.")
			: Result<string>.FailureResult(result.Errors.Select(e => e.Description).ToList());
	}

	public async Task<Result<List<string>>> GetUserRolesAsync(Guid userId)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user == null)
			return Result<List<string>>.FailureResult("User not found.");

		var roles = await _userManager.GetRolesAsync(user);
		return Result<List<string>>.SuccessResult(roles.ToList());
	}

	public async Task<Result<string>> RemoveUserFromRoleAsync(RemoveUserRoleRequestDto dto)
	{
		var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
		if (user == null)
			return Result<string>.FailureResult("User not found.");

		if (!await _userManager.IsInRoleAsync(user, dto.RoleName))
			return Result<string>.FailureResult("User is not in this role.");

		var result = await _userManager.RemoveFromRoleAsync(user, dto.RoleName);
		return result.Succeeded
			? Result<string>.SuccessResult("Role removed from user.")
			: Result<string>.FailureResult(result.Errors.Select(e => e.Description).ToList());
	}

	public async Task<Result<List<string>>> GetAllRolesAsync()
	{
		var roles = await Task.FromResult(_roleManager.Roles.Select(r => r.Name).ToList());
		return Result<List<string>>.SuccessResult(roles);
	}

	public async Task<Result<UserInfoDto>> GetUserByIdAsync(Guid userId)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user == null)
			return Result<UserInfoDto>.FailureResult("User not found.");

		var dto = new UserInfoDto
		{
			Id = user.Id,
			UserName = user.UserName,
			Email = user.Email,
			FirstName = user.FirstName,
			LastName = user.LastName,
			FullName = user.FullName,
			Bio = user.Bio,
			AvatarUrl = user.AvatarUrl,
			Location = user.Location,
			Website = user.Website,
			CreatedAt = user.CreatedAt
		};

		return Result<UserInfoDto>.SuccessResult(dto);
	}

	public async Task<Result<PagedResult<UserListItemDto>>> GetUsersAsync(UserListRequestDto request)
	{
		var query = _userManager.Users.AsQueryable();

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var search = request.Search.ToLower();
			query = query.Where(u =>
				u.UserName.ToLower().Contains(search) ||
				u.Email.ToLower().Contains(search));
		}

		var totalCount = await query.CountAsync();

		var users = await query
			.OrderBy(u => u.UserName)
			.Skip((request.Page - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync();

		var dtos = users.Select(u => new UserListItemDto
		{
			Id = u.Id,
			UserName = u.UserName,
			Email = u.Email
		}).ToList();

		return Result<PagedResult<UserListItemDto>>.SuccessResult(new PagedResult<UserListItemDto>(dtos, totalCount, request.Page, request.PageSize));
	}

	public async Task<Result<UserInfoDto>> GetUserByEmail(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
			return Result<UserInfoDto>.FailureResult("Email cannot be null or empty.");
		var user = await _userManager.FindByEmailAsync(email);
		if (user == null)
			return Result<UserInfoDto>.FailureResult("User not found.");
		var dto = new UserInfoDto
		{
			Id = user.Id,
			UserName = user.UserName,
			Email = user.Email
		};
		return Result<UserInfoDto>.SuccessResult(dto);
	}

	public string CreateToken(AppUser user, IList<string> roles)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
		};

		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _configuration["Jwt:Issuer"],
			audience: _configuration["Jwt:Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddHours(1),
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}