using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogusProject.Web.Controllers
{
	public class BaseController : Controller
	{
		protected string? AccessToken => HttpContext.Session.GetString("AccessToken");

		protected Guid? CurrentUserId
		{
			get
			{
				var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
				return Guid.TryParse(id, out var guid) ? guid : null;
			}
		}

		protected void SetAuthorizationHeader(HttpClient client)
		{
			if (!string.IsNullOrEmpty(AccessToken))
			{
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
			}
		}
		protected async Task<T?> ReadResponse<T>(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
				return default;

			return await response.Content.ReadFromJsonAsync<T>();
		}

		protected IActionResult HandleApiFailure(string message, object? model = null)
		{
			ModelState.AddModelError(string.Empty, message);
			return model is null ? RedirectToAction("Index") : View(model);
		}
	}
}
