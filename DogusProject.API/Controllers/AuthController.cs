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
		private readonly IAuthService _tokenService;
		private readonly IMapper _mapper;
		private readonly ILogger<AuthController> _logger;

		public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthService tokenService, IMapper mapper, ILogger<AuthController> logger, RoleManager<AppRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
			_mapper = mapper;
			_logger = logger;
			_roleManager = roleManager;
		}

		[HttpPost("register")]
		public async Task<ActionResult> Register(RegisterRequestDto registerDto)
		{
			var user = _mapper.Map<AppUser>(registerDto);

			var result = await _userManager.CreateAsync(user, registerDto.Password);
			if (!result.Succeeded)
				return BadRequest(Result.FailureResult(result.Errors.Select(e => e.Description).ToList()));

			if (!await _roleManager.RoleExistsAsync("User"))
				await _roleManager.CreateAsync(new AppRole { Name = "User" });

			await _userManager.AddToRoleAsync(user, "User");

			return Ok(Result.SuccessResult("Successful registration."));
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login(LoginRequestDto loginDto)
		{
			var user = await _userManager.FindByEmailAsync(loginDto.Email);
			if (user == null)
				return Unauthorized(Result.FailureResult("User not found."));

			var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
			if (!result.Succeeded)
				return Unauthorized(Result.FailureResult("Wrong password."));

			var roles = await _userManager.GetRolesAsync(user);
			var token = _tokenService.CreateToken(user, roles);

			var response = new AuthResponseDto
			{
				Token = token,
				UserName = user.UserName,
				Roles = roles.ToList()
			};

			return Ok(Result<AuthResponseDto>.SuccessResult(response));
		}
	}
}
