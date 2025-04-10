using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.DTOs;
using DogusProject.Infrastructure.Identity;
using DogusProject.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly IAuthService _authService;
		private readonly IMapper _mapper;
		private readonly ILogger<AuthController> _logger;

		public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthService authService, IMapper mapper, ILogger<AuthController> logger, RoleManager<AppRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_authService = authService;
			_mapper = mapper;
			_logger = logger;
			_roleManager = roleManager;
		}

		[HttpPost("register")]
		public async Task<ActionResult> Register(RegisterRequestDto registerDto)
		{
			var result = await _authService.RegisterAsync(registerDto);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login(LoginRequestDto loginDto)
		{
			var result = await _authService.LoginAsync(loginDto);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPost("updateUserInfo")]
		public async Task<ActionResult> UpdateUserInfo(UpdateUserInfoDto dto)
		{
			await _authService.UpdateUserInfoAsync(dto);
			return Ok(Result.SuccessResult("User information updated successfully."));
		}

		[HttpPost("changePassword")]
		public async Task<ActionResult> ChangePassword(Guid userId, string password, string newPassword)
		{
			await _authService.ChangePasswordAsync(userId, password, newPassword);
			return Ok(Result.SuccessResult("Password changed successfully."));
		}

		[HttpPost("create-role")]
		public async Task<IActionResult> CreateRole([FromBody] string roleName)
		{
			var result = await _authService.CreateRoleAsync(roleName);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPost("assign-role")]
		public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleRequestDto dto)
		{
			var result = await _authService.AssignRoleToUserAsync(dto);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("get-user-roles/{userId}")]
		public async Task<IActionResult> GetUserRoles(Guid userId)
		{
			var result = await _authService.GetUserRolesAsync(userId);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPost("remove-user-role")]
		public async Task<IActionResult> RemoveUserRole([FromBody] RemoveUserRoleRequestDto dto)
		{
			var result = await _authService.RemoveUserFromRoleAsync(dto);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("all-roles")]
		public async Task<IActionResult> GetAllRoles()
		{
			var result = await _authService.GetAllRolesAsync();
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPost("users")]
		public async Task<IActionResult> GetUsers([FromBody] UserListRequestDto request)
		{
			var result = await _authService.GetUsersAsync(request);
			return result.Success ? Ok(result) : BadRequest(result);
		}

	}
}
