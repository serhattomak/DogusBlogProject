using DogusProject.Application.Common;
using DogusProject.Application.DTOs;
using DogusProject.Infrastructure.Identity;
using DogusProject.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
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

	public AuthService(IConfiguration configuration, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<AuthService> logger)
	{
		_configuration = configuration;
		_roleManager = roleManager;
		_signInManager = signInManager;
		_userManager = userManager;
		_logger = logger;
	}

	public async Task<Result> RegisterAsync(RegisterRequestDto registerDto)
	{
		_logger.LogInformation("Attempting to register a new user...");
		var user = new AppUser
		{
			UserName = registerDto.UserName,
			Email = registerDto.Email,
			EmailConfirmed = true
		};
		var result = await _userManager.CreateAsync(user, registerDto.Password);
		if (!result.Succeeded)
		{
			_logger.LogError("User registration failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
			throw new Exception("User registration failed.");
		}
		await _userManager.AddToRoleAsync(user, "Kullanıcı");
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
			UserName = user.UserName,
			Roles = roles.ToList()
		};

		_logger.LogInformation("User logged in successfully.");
		return Result<AuthResponseDto>.SuccessResult(response);
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