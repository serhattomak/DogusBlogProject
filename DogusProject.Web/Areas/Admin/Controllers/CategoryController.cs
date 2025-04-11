using DogusProject.Web.Models.Category.DTOs;
using DogusProject.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace DogusProject.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly HttpClient _client;

		public CategoryController(IHttpClientFactory httpClientFactory)
		{
			_client = httpClientFactory.CreateClient("ApiClient");
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var response = await _client.GetAsync("category");
			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Kategoriler getirilemedi.";
				return View(new List<CategoryDto>());
			}

			var result = await response.Content.ReadFromJsonAsync<Result<List<CategoryDto>>>();
			return View(result?.Data ?? new List<CategoryDto>());
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateCategoryDto dto)
		{
			if (!ModelState.IsValid)
				return View(dto);

			var token = HttpContext.Session.GetString("AccessToken");

			if (string.IsNullOrEmpty(token))
			{
				ModelState.AddModelError(string.Empty, "Yetki bilgisi eksik. Lütfen tekrar giriş yapınız.");
				return View(dto);
			}

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.PostAsJsonAsync("category", dto);

			if (!response.IsSuccessStatusCode)
			{
				ModelState.AddModelError(string.Empty, "Kategori oluşturulamadı.");
				return View(dto);
			}

			return RedirectToAction("Index");
		}

		[HttpGet("edit/{id}")]
		public async Task<IActionResult> Edit(Guid id)
		{
			var response = await _client.GetAsync($"category/{id}");
			if (!response.IsSuccessStatusCode)
				return RedirectToAction("Index");

			var result = await response.Content.ReadFromJsonAsync<Result<CategoryDto>>();
			if (!result!.Success)
				return RedirectToAction("Index");

			return View(result.Data);
		}

		[HttpPost("category/edit/{id}")]
		public async Task<IActionResult> Edit(Guid id, CategoryDto dto)
		{
			if (!ModelState.IsValid)
				return View(dto);

			var token = HttpContext.Session.GetString("AccessToken");
			if (!string.IsNullOrEmpty(token))
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.PutAsJsonAsync("category", dto);
			if (!response.IsSuccessStatusCode)
			{
				ModelState.AddModelError(string.Empty, "Kategori güncellenemedi.");
				return View(dto);
			}

			return RedirectToAction("Index");
		}

		[HttpPost("category/delete/{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var token = HttpContext.Session.GetString("AccessToken");
			if (!string.IsNullOrEmpty(token))
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.DeleteAsync($"category/{id}");

			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Kategori silinemedi.";
			}

			return RedirectToAction("Index");
		}
	}
}
