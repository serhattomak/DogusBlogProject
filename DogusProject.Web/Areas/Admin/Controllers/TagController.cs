using DogusProject.Web.Controllers;
using DogusProject.Web.Models.Common;
using DogusProject.Web.Models.Tag.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Author,Admin")]
	[Route("admin/tag")]
	public class TagController : BaseController
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
			var result = await ReadResponse<Result<List<TagDto>>>(response);

			return View(result?.Data ?? new List<TagDto>());
		}

		[HttpGet("tag/create")]
		public IActionResult Create() => View();

		[HttpPost("tag/create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateTagDto dto)
		{
			if (!ModelState.IsValid)
				return View(dto);

			var response = await _client.PostAsJsonAsync("tag", dto);

			if (!response.IsSuccessStatusCode)
				return HandleApiFailure("Etiket oluşturulamadı.", dto);

			return RedirectToAction("Index");
		}

		[HttpGet("tag/edit/{id}")]
		public async Task<IActionResult> Edit(Guid id)
		{
			var response = await _client.GetAsync($"tag/{id}");
			var result = await ReadResponse<Result<TagDto>>(response);

			if (result?.Data == null)
				return RedirectToAction("Index");

			return View(result.Data);
		}

		[HttpPost("tag/edit/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, TagDto dto)
		{
			if (!ModelState.IsValid)
				return View(dto);

			var response = await _client.PutAsJsonAsync("tag", dto);
			if (!response.IsSuccessStatusCode)
				return HandleApiFailure("Etiket güncellenemedi.", dto);

			return RedirectToAction("Index");
		}

		[HttpPost("tag/delete/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Guid id)
		{
			var response = await _client.DeleteAsync($"tag/{id}");
			if (!response.IsSuccessStatusCode)
				TempData["Error"] = "Etiket silinemedi.";

			return RedirectToAction("Index");
		}
	}
}
