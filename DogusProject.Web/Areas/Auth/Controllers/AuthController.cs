using DogusProject.Application.Common;
using DogusProject.Web.Models.Auth.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogusProject.Web.Areas.Auth.Controllers
{
	[Area("Auth")]
	public class AuthController : Controller
	{
		private readonly HttpClient _client;

		public AuthController(IHttpClientFactory httpClientFactory)
		{
			_client = httpClientFactory.CreateClient("ApiClient");
		}

		[HttpGet("login")]
		public IActionResult Login() => View();

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginRequestDto dto)
		{
			Console.WriteLine($"DTO Email: {dto.Email}");
			Console.WriteLine($"DTO Password: {dto.Password}");

			if (!ModelState.IsValid)
				return View(dto);

			var response = await _client.PostAsJsonAsync("auth/login", dto);

			if (!response.IsSuccessStatusCode)
			{
				ModelState.AddModelError(string.Empty, "Giriş başarısız.");
				return View(dto);
			}

			var result = await response.Content.ReadFromJsonAsync<Result<AuthResponseDto>>();

			if (!result!.Success)
			{
				ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "Hata oluştu.");
				return View(dto);
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, result.Data.UserName),
				new Claim(ClaimTypes.NameIdentifier, result.Data.UserId.ToString())
			};

			result.Data.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

			return Redirect("/");
		}

		[HttpGet("register")]
		public IActionResult Register() => View();

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterRequestDto dto)
		{
			var response = await _client.PostAsJsonAsync("auth/register", dto);
			var result = await response.Content.ReadFromJsonAsync<Result<string>>();

			if (!result!.Success)
			{
				ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "Hata oluştu.");
				return View(dto);
			}

			return RedirectToAction("Login", "Auth", new { area = "Auth" });
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home", new { area = "" });
		}

		[HttpGet("settings")]
		public async Task<IActionResult> Settings()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			Console.WriteLine($"[SETTINGS] Claim UserId: {userId}");

			if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out _))
			{
				TempData["Error"] = "Giriş bilgileriniz alınamadı, lütfen yeniden giriş yapınız.";
				return RedirectToAction("Login", "Auth", new { area = "Auth" });
			}

			var response = await _client.GetAsync($"auth/user/{userId}");

			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Kullanıcı bilgileri alınamadı.";
				return RedirectToAction("Login", "Auth", new { area = "Auth" });
			}
			var result = await response.Content.ReadFromJsonAsync<Result<UpdateUserInfoDto>>();

			if (!result!.Success)
			{
				TempData["Error"] = result.Errors.FirstOrDefault();
				return RedirectToAction("Login", "Auth", new { area = "Auth" });
			}

			return View(result.Data);
		}

		[HttpPost("settings")]
		public async Task<IActionResult> Settings(UpdateUserInfoDto dto)
		{
			var rawUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			Console.WriteLine($"[DEBUG] NameIdentifier Claim: {rawUserId}");

			if (!ModelState.IsValid)
				return View(dto);

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
			{
				ModelState.AddModelError(string.Empty, "Kullanıcı bilgisi alınamadı.");
				return View(dto);
			}

			dto.UserId = Guid.Parse(userId);

			var response = await _client.PostAsJsonAsync("auth/updateUserInfo", dto);

			if (!response.IsSuccessStatusCode)
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				ModelState.AddModelError(string.Empty, $"Güncelleme başarısız. [Status: {(int)response.StatusCode}] {errorContent}");
				return View(dto);
			}

			var result = await response.Content.ReadFromJsonAsync<Result>();

			if (!result!.Success)
			{
				ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault() ?? "Güncelleme başarısız.");
				return View(dto);
			}

			TempData["Success"] = "Bilgiler başarıyla güncellendi.";
			return RedirectToAction("Settings");
		}

		[HttpGet("change-password")]
		public IActionResult ChangePassword() => View();

		[HttpPost("change-password")]
		public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto dto)
		{
			if (!ModelState.IsValid)
				return View(dto);

			var userIdRaw = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userIdRaw) || !Guid.TryParse(userIdRaw, out var userId))
			{
				TempData["Error"] = "Kullanıcı bilgisi alınamadı.";
				return View(dto);
			}

			var dtoToSend = new ChangePasswordRequestDto
			{
				UserId = userId,
				CurrentPassword = dto.CurrentPassword,
				NewPassword = dto.NewPassword
			};

			var response = await _client.PostAsJsonAsync("auth/changePassword", dtoToSend);

			if (!response.IsSuccessStatusCode)
			{
				var error = await response.Content.ReadAsStringAsync();
				TempData["Error"] = $"Hata: {error}";
				return View(dto);
			}

			var result = await response.Content.ReadFromJsonAsync<Result>();
			if (!result!.Success)
			{
				TempData["Error"] = result.Errors.FirstOrDefault();
				return View(dto);
			}

			TempData["Success"] = "Şifreniz başarıyla güncellendi.";
			return RedirectToAction("ChangePassword");
		}

	}
}
