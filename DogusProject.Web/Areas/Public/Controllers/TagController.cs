using DogusProject.Web.Models.Blog.DTOs;
using DogusProject.Web.Models.Common;
using DogusProject.Web.Models.Tag.DTOs;
using DogusProject.Web.Models.Tag.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.Web.Areas.Public.Controllers
{
	[Area("Public")]
	[Route("tags")]
	public class TagController : Controller
	{
		private readonly HttpClient _client;

		public TagController(IHttpClientFactory factory)
		{
			_client = factory.CreateClient("ApiClient");
		}

		[HttpGet("")]
		public async Task<IActionResult> Index()
		{
			var response = await _client.GetAsync("tag");
			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Etiketler alınamadı.";
				return View(new List<TagDto>());
			}

			var result = await response.Content.ReadFromJsonAsync<Result<List<TagDto>>>();
			return View(result?.Data ?? new List<TagDto>());
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> BlogsByTag(Guid id, int page = 1, int pageSize = 10)
		{
			var response = await _client.GetAsync($"blog/by-tag/{id}?page={page}&pageSize={pageSize}");
			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Bloglar getirilemedi.";
				return View(new BlogsByTagViewModel { TagId = id });
			}

			var result = await response.Content.ReadFromJsonAsync<Result<PagedResult<BlogResponseDto>>>();

			var vm = new BlogsByTagViewModel
			{
				TagId = id,
				PagedBlogs = result!.Data
			};

			return View(vm);
		}
	}
}
