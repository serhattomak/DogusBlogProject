using DogusProject.Web.Controllers;
using DogusProject.Web.Models.Blog.DTOs;
using DogusProject.Web.Models.Category.DTOs;
using DogusProject.Web.Models.Category.ViewModels;
using DogusProject.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.Web.Areas.Public.Controllers
{
	[Area("Public")]
	[Route("category")]
	public class CategoryController : BaseController
	{
		private readonly HttpClient _client;

		public CategoryController(IHttpClientFactory factory)
		{
			_client = factory.CreateClient("ApiClient");
		}

		[HttpGet("")]
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

		[HttpGet("{id}")]
		public async Task<IActionResult> BlogsByCategory(Guid id, int page = 1, int pageSize = 10)
		{
			var response = await _client.GetAsync($"blog/by-category/{id}?page={page}&pageSize={pageSize}");
			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Bloglar getirilemedi.";
				return View(new BlogsByCategoryViewModel { CategoryId = id });
			}

			var result = await response.Content.ReadFromJsonAsync<Result<PagedResult<BlogResponseDto>>>();

			var vm = new BlogsByCategoryViewModel
			{
				CategoryId = id,
				PagedBlogs = result!.Data
			};

			return View(vm);
		}
	}
}
