using DogusProject.Web.Models.Blog.ViewModels;
using DogusProject.Web.Models.Category.DTOs;
using DogusProject.Web.Models.Common;
using DogusProject.Web.Models.Tag.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using BlogResponseDto = DogusProject.Web.Models.Blog.DTOs.BlogResponseDto;
using CreateBlogDto = DogusProject.Web.Models.Blog.DTOs.CreateBlogDto;
using UpdateBlogDto = DogusProject.Web.Models.Blog.DTOs.UpdateBlogDto;

namespace DogusProject.Web.Areas.Author.Controllers
{
	[Area("Author")]
	[Route("author/blog")]
	public class BlogController : Controller
	{
		private readonly HttpClient _client;

		public BlogController(IHttpClientFactory httpClientFactory)
		{
			_client = httpClientFactory.CreateClient("ApiClient");
		}

		[HttpGet]
		[Authorize(Roles = "Author,Admin")]
		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
				return Unauthorized();
			var token = HttpContext.Session.GetString("AccessToken");
			if (!string.IsNullOrEmpty(token))
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.GetAsync($"blog/by-author/{userId}?page={page}&pageSize={pageSize}");
			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Bloglar getirilemedi.";
				return View(new PagedResult<BlogResponseDto>([], 0, page, pageSize));
			}
			var result = await response.Content.ReadFromJsonAsync<Result<PagedResult<BlogResponseDto>>>();
			return View(result!.Data);
		}

		[HttpGet("my-blogs")]
		[Authorize(Roles = "Author,Admin")]
		public async Task<IActionResult> MyBlogs(int page = 1, int pageSize = 10)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
				return Unauthorized();

			var response = await _client.GetAsync($"blog/by-author/{userId}?page={page}&pageSize={pageSize}");
			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Bloglar getirilemedi.";
				return View(new PagedResult<BlogResponseDto>([], 0, page, pageSize));
			}

			var result = await response.Content.ReadFromJsonAsync<Result<PagedResult<BlogResponseDto>>>();
			return View(result!.Data);
		}


		[HttpGet("create")]
		public async Task<IActionResult> Create()
		{
			var categories = await _client.GetFromJsonAsync<Result<List<CategoryDto>>>("category");
			var tags = await _client.GetFromJsonAsync<Result<List<TagDto>>>("tag");

			var model = new CreateBlogViewModel
			{
				Categories = categories?.Data ?? new(),
				Tags = tags?.Data ?? new()
			};

			return View(model);
		}

		[HttpPost("create")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Author,Admin")]
		public async Task<IActionResult> Create(CreateBlogViewModel model)
		{
			if (!ModelState.IsValid)
			{
				await PrepareViewModelAsync(model);
				return View(model);
			}

			var token = HttpContext.Session.GetString("AccessToken");
			if (string.IsNullOrEmpty(token))
				return Unauthorized();

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var createdTagIds = new List<Guid>();

			if (!string.IsNullOrWhiteSpace(model.NewTags))
			{
				var newTagNames = model.NewTags
					.Split(',', StringSplitOptions.RemoveEmptyEntries)
					.Select(t => t.Trim())
					.Where(t => !string.IsNullOrWhiteSpace(t))
					.ToList();

				foreach (var tagName in newTagNames)
				{
					var tagCreateDto = new CreateTagDto { Name = tagName };
					var tagResponse = await _client.PostAsJsonAsync("tag", tagCreateDto);
					if (tagResponse.IsSuccessStatusCode)
					{
						var created = await tagResponse.Content.ReadFromJsonAsync<Result<Guid>>();
						if (created != null && created.Success)
							createdTagIds.Add(created.Data);
					}
				}
			}

			var allSelectedTags = model.SelectedTagIds.Concat(createdTagIds).ToList();

			var dto = new CreateBlogDto
			{
				Title = model.Title,
				Content = model.Content,
				CategoryId = model.CategoryId,
				TagIds = allSelectedTags
			};

			var response = await _client.PostAsJsonAsync("blog/create", dto);
			if (!response.IsSuccessStatusCode)
			{
				ModelState.AddModelError(string.Empty, "Blog oluşturulamadı.");
				await PrepareViewModelAsync(model);
				return View(model);
			}

			return RedirectToAction("Index", "Blog");
		}

		[HttpGet("edit/{id}")]
		[Authorize(Roles = "Author,Admin")]
		public async Task<IActionResult> Edit(Guid id)
		{
			var blogResponse = await _client.GetAsync($"blog/{id}");
			if (!blogResponse.IsSuccessStatusCode)
			{
				TempData["Error"] = "Blog bulunamadı.";
				return RedirectToAction("Index");
			}

			var blogResult = await blogResponse.Content.ReadFromJsonAsync<Result<UpdateBlogDto>>();
			if (blogResult?.Data == null)
			{
				TempData["Error"] = "Blog bilgileri alınamadı.";
				return RedirectToAction("Index");
			}

			var categoriesResponse = await _client.GetAsync("category");
			var tagsResponse = await _client.GetAsync("tag");

			var categoryResult = await categoriesResponse.Content.ReadFromJsonAsync<Result<List<CategoryDto>>>();
			var tagResult = await tagsResponse.Content.ReadFromJsonAsync<Result<List<TagDto>>>();

			var model = new UpdateBlogViewModel
			{
				Id = blogResult.Data.Id,
				Title = blogResult.Data.Title,
				Content = blogResult.Data.Content,
				CategoryId = blogResult.Data.CategoryId,
				SelectedTagIds = blogResult.Data.TagIds,
				Categories = categoryResult?.Data ?? new(),
				AvailableTags = tagResult?.Data ?? new()
			};

			return View(model);
		}

		[HttpPost("edit/{id}")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Author,Admin")]
		public async Task<IActionResult> Edit(Guid id, UpdateBlogViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var token = HttpContext.Session.GetString("AccessToken");
			if (!string.IsNullOrEmpty(token))
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			if (!string.IsNullOrWhiteSpace(model.NewTags))
			{
				var newTagNames = model.NewTags
					.Split(',', StringSplitOptions.RemoveEmptyEntries)
					.Select(t => t.Trim())
					.Where(t => !string.IsNullOrWhiteSpace(t))
					.ToList();

				foreach (var tagName in newTagNames)
				{
					var tagCreateDto = new CreateTagDto { Name = tagName };
					var tagResponse = await _client.PostAsJsonAsync("tag", tagCreateDto);
					if (tagResponse.IsSuccessStatusCode)
					{
						var created = await tagResponse.Content.ReadFromJsonAsync<Result<Guid>>();
						if (created != null && created.Success)
							model.SelectedTagIds.Add(created.Data);
					}
				}
			}

			var dto = new UpdateBlogDto
			{
				Id = id,
				Title = model.Title,
				Content = model.Content,
				CategoryId = model.CategoryId,
				TagIds = model.SelectedTagIds
			};

			var response = await _client.PutAsJsonAsync("blog", dto);
			if (!response.IsSuccessStatusCode)
			{
				ModelState.AddModelError(string.Empty, "Blog güncellenemedi.");
				return View(model);
			}

			return RedirectToAction("Index");
		}

		[HttpPost("delete/{id}")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Author,Admin")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var token = HttpContext.Session.GetString("AccessToken");
			if (!string.IsNullOrEmpty(token))
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.DeleteAsync($"blog/{id}");
			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Blog silinemedi.";
			}

			return RedirectToAction("Index");
		}


		private async Task<CreateBlogViewModel> PrepareViewModelAsync(CreateBlogViewModel model)
		{
			var response = await _client.GetAsync("tag");
			if (!response.IsSuccessStatusCode)
			{
				model.AvailableTags = new List<TagDto>();
				return model;
			}

			var tagResult = await response.Content.ReadFromJsonAsync<Result<List<TagDto>>>();
			model.AvailableTags = tagResult?.Data ?? new List<TagDto>();
			return model;
		}

	}

}
