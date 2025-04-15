using DogusProject.Application.Features.BlogImages.Dtos;
using DogusProject.Web.Controllers;
using DogusProject.Web.Models.Blog.ViewModels;
using DogusProject.Web.Models.Category.DTOs;
using DogusProject.Web.Models.Common;
using DogusProject.Web.Models.Tag.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogResponseDto = DogusProject.Web.Models.Blog.DTOs.BlogResponseDto;
using CreateBlogDto = DogusProject.Web.Models.Blog.DTOs.CreateBlogDto;
using UpdateBlogDto = DogusProject.Web.Models.Blog.DTOs.UpdateBlogDto;

namespace DogusProject.Web.Areas.Author.Controllers
{
	[Area("Author")]
	[Authorize(Roles = "Author,Admin")]
	[Route("author/blog")]
	public class BlogController : BaseController
	{
		private readonly HttpClient _client;

		public BlogController(IHttpClientFactory httpClientFactory)
		{
			_client = httpClientFactory.CreateClient("ApiClient");
		}

		[HttpGet]
		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var userId = CurrentUserId;
			if (userId == Guid.Empty)
				return Unauthorized();

			var response = await _client.GetAsync($"blog/by-author/{userId}?page={page}&pageSize={pageSize}");
			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Bloglar getirilemedi.";
				return View(new PagedResult<BlogResponseDto>([], 0, page, pageSize));
			}
			var result = await ReadResponse<Result<PagedResult<BlogResponseDto>>>(response);

			if (result == null || !result.Success)
				return View(new PagedResult<BlogResponseDto>([], 0, page, pageSize));

			return View(result.Data);
		}

		[HttpGet("my-blogs")]
		public async Task<IActionResult> MyBlogs(int page = 1, int pageSize = 10)
		{
			var userId = CurrentUserId;
			if (userId == Guid.Empty)
				return Unauthorized();

			var response = await _client.GetAsync($"blog/by-author/{userId}?page={page}&pageSize={pageSize}");
			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Bloglar getirilemedi.";
				return View(new PagedResult<BlogResponseDto>([], 0, page, pageSize));
			}

			var result = await ReadResponse<Result<PagedResult<BlogResponseDto>>>(response);
			if (result == null || !result.Success)
				return View(new PagedResult<BlogResponseDto>([], 0, page, pageSize));
			return View(result.Data);
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
		public async Task<IActionResult> Create(CreateBlogViewModel model)
		{
			if (!ModelState.IsValid)
			{
				await PrepareViewModelAsync(model);
				return View(model);
			}

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

			var createdBlogResult = await response.Content.ReadFromJsonAsync<Result<Guid>>();
			if (createdBlogResult == null || !createdBlogResult.Success)
				return RedirectToAction("Index");

			if (model.Images != null && model.Images.Any())
			{
				using var content = new MultipartFormDataContent();
				foreach (var image in model.Images)
				{
					var streamContent = new StreamContent(image.OpenReadStream());
					streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
					content.Add(streamContent, "images", image.FileName);
				}

				var uploadResponse = await _client.PostAsync($"blogimage/upload/{createdBlogResult.Data}", content);

				if (!uploadResponse.IsSuccessStatusCode)
				{
					var errorText = await uploadResponse.Content.ReadAsStringAsync();
					Console.WriteLine("Görsel yükleme hatası: " + errorText);
				}

			}

			return RedirectToAction("Index", "Blog");
		}

		[HttpGet("edit/{id}")]
		public async Task<IActionResult> Edit(Guid id)
		{
			var blogResponse = await _client.GetAsync($"blog/for-edit/{id}");
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

			var imageResponse = await _client.GetAsync($"blogimage/by-blog/{id}");
			var imageList = new List<(Guid Id, string Url)>();
			if (imageResponse.IsSuccessStatusCode)
			{
				var result = await imageResponse.Content.ReadFromJsonAsync<Result<List<BlogImageDto>>>();
				if (result?.Success == true)
					imageList = result.Data.Select(img => (img.Id, img.ImageUrl)).ToList();
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
				AvailableTags = tagResult?.Data ?? new(),
				ExistingImages = imageList
			};

			return View(model);
		}

		[HttpPost("edit/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, UpdateBlogViewModel model)
		{

			if (!ModelState.IsValid)
			{
				foreach (var kvp in ModelState)
				{
					foreach (var error in kvp.Value.Errors)
					{
						Console.WriteLine($"Hata: {kvp.Key} => {error.ErrorMessage}");
					}
				}
			}

			Console.WriteLine("Seçilen Tag Id'ler:");
			foreach (var tag in model.SelectedTagIds)
			{
				Console.WriteLine(tag);
			}

			if (!ModelState.IsValid)
			{
				await PrepareViewModelAsync(model, id);
				return View(model);
			}

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
				TagIds = model.SelectedTagIds.Distinct().ToList()
			};

			var response = await _client.PutAsJsonAsync("blog", dto);
			if (!response.IsSuccessStatusCode)
			{
				await PrepareViewModelAsync(model, id);
				ModelState.AddModelError(string.Empty, "Blog güncellenemedi.");
				return View(model);
			}

			if (model.Images != null && model.Images.Any())
			{
				using var content = new MultipartFormDataContent();
				foreach (var image in model.Images)
				{
					var streamContent = new StreamContent(image.OpenReadStream());
					streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
					content.Add(streamContent, "images", image.FileName);
				}

				var uploadResponse = await _client.PostAsync($"blogimage/upload/{id}", content);
				if (!uploadResponse.IsSuccessStatusCode)
				{
					var errorText = await uploadResponse.Content.ReadAsStringAsync();
					Console.WriteLine("Görsel yükleme hatası: " + errorText);
				}
			}

			TempData["SuccessMessage"] = "Blog başarıyla güncellendi.";
			return RedirectToAction("Edit", new { id });
		}


		[HttpPost("delete/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Guid id)
		{

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

		private async Task PrepareViewModelAsync(UpdateBlogViewModel model, Guid blogId)
		{
			var categoriesResponse = await _client.GetAsync("category");
			var tagsResponse = await _client.GetAsync("tag");
			var imagesResponse = await _client.GetAsync($"blogimage/by-blog/{blogId}");

			var categoryResult = await categoriesResponse.Content.ReadFromJsonAsync<Result<List<CategoryDto>>>();
			var tagResult = await tagsResponse.Content.ReadFromJsonAsync<Result<List<TagDto>>>();
			var imageResult = await imagesResponse.Content.ReadFromJsonAsync<Result<List<BlogImageDto>>>();

			model.Categories = categoryResult?.Data ?? new();
			model.AvailableTags = tagResult?.Data ?? new();
			model.ExistingImages = imageResult?.Data?.Select(x => (x.Id, x.ImageUrl)).ToList() ?? new();
		}
	}

}