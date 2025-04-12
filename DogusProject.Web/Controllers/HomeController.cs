using DogusProject.Web.Models;
using DogusProject.Web.Models.Blog.DTOs;
using DogusProject.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DogusProject.Web.Controllers
{
	public class HomeController : BaseController
	{
		private readonly HttpClient _client;

		public HomeController(IHttpClientFactory factory)
		{
			_client = factory.CreateClient("ApiClient");
		}

		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var response = await _client.GetAsync($"blog/all?page={page}&pageSize={pageSize}");
			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Bloglar getirilemedi.";
				return View(new PagedResult<BlogResponseDto>([], 0, page, pageSize));
			}

			var result = await ReadResponse<Result<PagedResult<BlogResponseDto>>>(response);
			return View(result?.Data ?? new PagedResult<BlogResponseDto>([], 0, page, pageSize));
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
