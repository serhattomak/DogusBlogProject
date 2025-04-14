using DogusProject.Application.Common;
using DogusProject.Application.Features.Users.Dtos;
using DogusProject.Web.Controllers;
using DogusProject.Web.Models.Auth.DTOs;
using DogusProject.Web.Models.Auth.ViewModels;
using DogusProject.Web.Models.Blog.DTOs;
using DogusProject.Web.Models.Comment.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace DogusProject.Web.Areas.Auth.Controllers
{
	[Area("Auth")]
	public class AuthController : BaseController
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

			HttpContext.Session.SetString("AccessToken", result.Data.Token);

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

		[HttpGet("profile")]
		public async Task<IActionResult> Profile()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
				return RedirectToAction("Login", "Auth", new { area = "Auth" });

			var token = HttpContext.Session.GetString("AccessToken");
			if (string.IsNullOrEmpty(token))
			{
				TempData["Error"] = "Token bulunamadı. Lütfen tekrar giriş yapın.";
				return RedirectToAction("Login", "Auth", new { area = "Auth" });
			}

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			Result<UserInfoDto>? userResult = null;
			Result<List<string>>? rolesResult = null;
			Result<List<BlogResponseDto>>? blogsResult = null;
			Result<List<CommentResponseDto>>? commentsResult = null;

			try
			{
				var userResponse = await _client.GetAsync($"auth/user/{userId}");
				userResponse.EnsureSuccessStatusCode();
				userResult = await userResponse.Content.ReadFromJsonAsync<Result<UserInfoDto>>();

				var rolesResponse = await _client.GetAsync($"auth/get-user-roles/{userId}");
				rolesResponse.EnsureSuccessStatusCode();
				rolesResult = await rolesResponse.Content.ReadFromJsonAsync<Result<List<string>>>();

				var blogsResponse = await _client.GetAsync($"blog/by-author-info/{userId}");
				blogsResponse.EnsureSuccessStatusCode();
				blogsResult = await blogsResponse.Content.ReadFromJsonAsync<Result<List<BlogResponseDto>>>();

				var commentsResponse = await _client.GetAsync($"comment/by-user/{userId}");
				commentsResponse.EnsureSuccessStatusCode();
				commentsResult = await commentsResponse.Content.ReadFromJsonAsync<Result<List<CommentResponseDto>>>();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[Profile Error] {ex.Message}");
				TempData["Error"] = "Profil verileri alınamadı.";
				return RedirectToAction("Index", "Home", new { area = "" });
			}

			var model = new UserProfileViewModel
			{
				UserName = userResult?.Data?.UserName ?? "Bilinmiyor",
				Email = userResult?.Data?.Email ?? "Bilinmiyor",
				FullName = userResult?.Data?.FullName ?? "",
				FirstName = userResult?.Data?.FirstName ?? "",
				LastName = userResult?.Data?.LastName ?? "",
				Bio = userResult?.Data?.Bio,
				Location = userResult?.Data?.Location,
				Website = userResult?.Data?.Website,
				AvatarUrl = userResult?.Data?.AvatarUrl ?? "/images/default-avatar.png",
				JoinedDate = userResult?.Data?.CreatedAt ?? DateTime.UtcNow,
				Blogs = blogsResult?.Data ?? new(),
				Comments = commentsResult?.Data ?? new(),
				PostCount = blogsResult?.Data?.Count ?? 0
			};

			return View(model);
		}

		[HttpGet("edit-profile")]
		public async Task<IActionResult> EditProfile()
		{
			var userId = CurrentUserId;
			var profileResponse = await _client.GetAsync($"user/profile/{userId}");

			if (!profileResponse.IsSuccessStatusCode)
			{
				TempData["Error"] = "Profil bilgileri alınamadı.";
				return RedirectToAction("Profile");
			}

			var profileResult = await profileResponse.Content.ReadFromJsonAsync<Result<UserProfileDto>>();

			var model = new EditProfileViewModel
			{
				UserId = (Guid)userId,
				FirstName = profileResult.Data.FirstName,
				LastName = profileResult.Data.LastName,
				Bio = profileResult.Data.Bio,
				Location = profileResult.Data.Location,
				Website = profileResult.Data.Website
			};

			return View(model);
		}

		[HttpPost("edit-profile")]
		public async Task<IActionResult> EditProfile(EditProfileViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var dto = new UpdateProfileDto
			{
				UserId = model.UserId,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Bio = model.Bio,
				Location = model.Location,
				Website = model.Website
			};

			SetAuthorizationHeader(_client);
			var response = await _client.PutAsJsonAsync("user/edit-profile", dto);

			if (!response.IsSuccessStatusCode)
			{
				ModelState.AddModelError(string.Empty, "Profil güncellenemedi.");
				return HandleApiFailure("Profil güncellenemedi.", response);
			}

			return RedirectToAction("Profile");
		}

		[HttpPost("upload-avatar")]
		public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarRequest request)
		{
			var token = HttpContext.Session.GetString("AccessToken");
			if (string.IsNullOrEmpty(token))
			{
				TempData["Error"] = "Token bulunamadı. Lütfen tekrar giriş yapın.";
				return RedirectToAction("Profile");
			}

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var form = new MultipartFormDataContent
			{
				{ new StringContent(request.UserId.ToString()), "UserId" },
				{ new StreamContent(request.File.OpenReadStream()), "File", request.File.FileName }
			};

			var response = await _client.PostAsync("user/upload-avatar", form);
			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Fotoğraf yüklenemedi.";
				return RedirectToAction("Profile");
			}

			TempData["Success"] = "Profil fotoğrafı güncellendi.";
			return RedirectToAction("Profile");
		}

	}
}
