using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Web.Controllers;
using DogusProject.Web.Models.Blog.ViewModels;
using DogusProject.Web.Models.Comment.DTOs;
using DogusProject.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;
using BlogResponseDto = DogusProject.Web.Models.Blog.DTOs.BlogResponseDto;

namespace DogusProject.Web.Areas.Public.Controllers
{
	[Area("Public")]
	[Route("blog")]
	public class BlogController : BaseController
	{
		private readonly HttpClient _client;

		public BlogController(IHttpClientFactory factory)
		{
			_client = factory.CreateClient("ApiClient");
		}

		[HttpGet("")]
		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var response = await _client.GetAsync($"blog/all?page={page}&pageSize={pageSize}");
			var result = await ReadResponse<Result<PagedResult<BlogResponseDto>>>(response);

			if (result is null || !result.Success)
				return View(new PagedResult<BlogResponseDto>([], 0, page, pageSize));

			return View(result.Data);
		}

		[HttpGet("detail/{id}")]
		public async Task<IActionResult> Detail(Guid id)
		{
			var blogResponse = await _client.GetAsync($"blog/{id}");

			if (!blogResponse.IsSuccessStatusCode)
			{
				TempData["Error"] = "Blog bulunamadı.";
				return RedirectToAction("Index");
			}

			var blogResult = await blogResponse.Content.ReadFromJsonAsync<Result<BlogDetailDto>>();
			if (blogResult?.Data == null)
			{
				TempData["Error"] = "Blog detayları alınamadı.";
				return RedirectToAction("Index");
			}

			var commentsResponse = await _client.GetAsync($"comment/by-blog/{id}");
			var commentsResult = await commentsResponse.Content.ReadFromJsonAsync<Result<List<CommentResponseDto>>>();

			var model = new BlogDetailViewModel
			{
				Blog = blogResult.Data,
				Comments = commentsResult?.Data ?? new List<CommentResponseDto>(),
				NewComment = new CreateCommentDto { BlogId = id }
			};

			return View(model);
		}

		[HttpPost("detail/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Detail(Guid id, IFormCollection form)
		{
			var userId = CurrentUserId;
			if (userId == Guid.Empty)
			{
				TempData["Error"] = "Yorum yapabilmek için giriş yapmalısınız.";
				return RedirectToAction("Detail", new { id });
			}

			var content = form["Content"].ToString();

			if (string.IsNullOrWhiteSpace(content))
			{
				ModelState.AddModelError("Content", "Yorum içeriği boş olamaz.");
			}

			if (!ModelState.IsValid)
			{
				var blogResult = await _client.GetFromJsonAsync<Result<BlogDetailDto>>($"blog/{id}");
				var commentsResult = await _client.GetFromJsonAsync<Result<List<CommentResponseDto>>>($"comment/by-blog/{id}");

				var vm = new BlogDetailViewModel
				{
					Blog = blogResult?.Data!,
					Comments = commentsResult?.Data ?? new(),
					NewComment = new CreateCommentDto
					{
						BlogId = id,
						Content = content
					}
				};

				return View(vm);
			}

			var commentDto = new CreateCommentDto
			{
				BlogId = id,
				UserId = (Guid)userId,
				Content = content
			};

			var response = await _client.PostAsJsonAsync("comment", commentDto);

			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Yorum kaydedilemedi.";
			}

			return RedirectToAction("Detail", new { id });
		}
	}
}
