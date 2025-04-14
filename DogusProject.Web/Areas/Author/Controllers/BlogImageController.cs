using DogusProject.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.Web.Areas.Author.Controllers
{
	[Area("Author")]
	[Authorize(Roles = "Author,Admin")]
	[Route("author/blogimage")]
	public class BlogImageController : BaseController
	{
		private readonly HttpClient _client;

		public BlogImageController(IHttpClientFactory httpClientFactory)
		{
			_client = httpClientFactory.CreateClient("ApiClient");
		}

		[HttpPost("delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Guid id)
		{
			var response = await _client.DeleteAsync($"blogimage/{id}");
			if (!response.IsSuccessStatusCode)
			{
				TempData["Error"] = "Görsel silinemedi.";
			}

			return Redirect(Request.Headers["Referer"].ToString());
		}
	}
}
