using DogusProject.Web.Controllers;
using DogusProject.Web.Models.Category.DTOs;
using DogusProject.Web.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CategoryController : BaseController
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

			var result = await ReadResponse<Result<List<CategoryDto>>>(response);
			if (result == null || !result.Success)
			{
				TempData["Error"] = "Kategoriler getirilemedi.";
				return View(new List<CategoryDto>());
			}

			return View(result.Data ?? new());
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateCategoryDto dto)
		{
			if (!ModelState.IsValid)
				return View(dto);

			var response = await _client.PostAsJsonAsync("category", dto);

			if (!response.IsSuccessStatusCode)
				return HandleApiFailure("Kategori oluşturulamadı.", dto);

			return RedirectToAction("Index");
		}

		[HttpGet("category/edit/{id}")]
		public async Task<IActionResult> Edit(Guid id)
		{
			var response = await _client.GetAsync($"category/{id}");
			if (!response.IsSuccessStatusCode)
				return RedirectToAction("Index");

			var result = await ReadResponse<Result<CategoryDto>>(response);
			if (result == null || !result.Success)
				return RedirectToAction("Index");

			return View(result.Data);
		}

		[HttpPost("category/edit/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, CategoryDto dto)
		{
			if (!ModelState.IsValid)
				return View(dto);

			SetAuthorizationHeader(_client);

			var response = await _client.PutAsJsonAsync("category", dto);
			if (!response.IsSuccessStatusCode)
				return HandleApiFailure("Kategori güncellenemedi.", dto);

			return RedirectToAction("Index");
		}

		[HttpPost("category/delete/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Guid id)
		{
			SetAuthorizationHeader(_client);

			var response = await _client.DeleteAsync($"category/{id}");

			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Kategori silinemedi.";
			}

			return RedirectToAction("Index");
		}
	}
}
