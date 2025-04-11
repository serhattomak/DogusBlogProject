using DogusProject.Web.Models.Common;
using DogusProject.Web.Models.Tag.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace DogusProject.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Author,Admin")]
	public class TagController : Controller
	{
		private readonly HttpClient _client;

		public TagController(IHttpClientFactory httpClientFactory)
		{
			_client = httpClientFactory.CreateClient("ApiClient");
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var response = await _client.GetAsync("tag");
			if (!response.IsSuccessStatusCode)
				return View(new List<TagDto>());

			var result = await response.Content.ReadFromJsonAsync<Result<List<TagDto>>>();
			return View(result?.Data ?? new List<TagDto>());
		}

		[HttpGet]
		public IActionResult Create() => View();

		[HttpPost]
		public async Task<IActionResult> Create(CreateTagDto dto)
		{
			if (!ModelState.IsValid)
				return View(dto);

			var token = HttpContext.Session.GetString("AccessToken");
			if (!string.IsNullOrEmpty(token))
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.PostAsJsonAsync("tag", dto);

			if (!response.IsSuccessStatusCode)
			{
				ModelState.AddModelError(string.Empty, "Etiket oluşturulamadı.");
				return View(dto);
			}

			return RedirectToAction("Index");
		}

		[HttpGet("tag/edit/{id}")]
		public async Task<IActionResult> Edit(Guid id)
		{
			var response = await _client.GetAsync($"tag/{id}");
			if (!response.IsSuccessStatusCode) return RedirectToAction("Index");

			var result = await response.Content.ReadFromJsonAsync<Result<TagDto>>();
			return View(result?.Data);
		}

		[HttpPost("tag/edit/{id}")]
		public async Task<IActionResult> Edit(Guid id, TagDto dto)
		{
			if (!ModelState.IsValid) return View(dto);

			var token = HttpContext.Session.GetString("AccessToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.PutAsJsonAsync("tag", dto);
			if (!response.IsSuccessStatusCode)
			{
				ModelState.AddModelError(string.Empty, "Etiket güncellenemedi.");
				return View(dto);
			}

			return RedirectToAction("Index");
		}

		[HttpPost("tag/delete/{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var token = HttpContext.Session.GetString("AccessToken");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.DeleteAsync($"tag/{id}");
			if (!response.IsSuccessStatusCode)
				TempData["Error"] = "Etiket silinemedi.";

			return RedirectToAction("Index");
		}
	}
}
