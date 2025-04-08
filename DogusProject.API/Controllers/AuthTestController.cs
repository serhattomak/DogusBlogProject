using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthTestController : ControllerBase
	{
		[HttpGet("anonymous")]
		[AllowAnonymous]
		public IActionResult AnonymousEndpoint()
		{
			return Ok(new { message = "Anonymous endpoint works" });
		}

		[HttpGet("authenticated")]
		[Authorize]
		public IActionResult AuthenticatedEndpoint()
		{
			return Ok(new
			{
				message = "Authenticated endpoint works",
				user = User.Identity?.Name,
				isAuthenticated = User.Identity?.IsAuthenticated,
				claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
			});
		}

		[HttpGet("admin")]
		[Authorize(Roles = "Admin")]
		public IActionResult AdminEndpoint()
		{
			return Ok(new
			{
				message = "Admin endpoint works",
				user = User.Identity?.Name,
				roles = User.Claims.Where(c => c.Type.Contains("role")).Select(c => c.Value).ToList()
			});
		}

		[HttpGet("debug-auth")]
		[AllowAnonymous]
		public IActionResult DebugAuth()
		{
			// Get authorization header
			var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

			return Ok(new
			{
				hasAuthHeader = !string.IsNullOrEmpty(authHeader),
				authHeaderValue = !string.IsNullOrEmpty(authHeader) ? $"{authHeader.Substring(0, Math.Min(20, authHeader.Length))}..." : null,
				isAuthenticated = User.Identity?.IsAuthenticated,
				authenticationType = User.Identity?.AuthenticationType,
				userName = User.Identity?.Name,
				claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
			});
		}

		[HttpGet("token-test")]
		[AllowAnonymous]
		public IActionResult TokenTest()
		{
			var authHeader = Request.Headers["Authorization"].ToString();
			return Ok(new
			{
				RawHeader = authHeader,
				IsAuthenticated = User.Identity?.IsAuthenticated,
				Claims = User.Claims.Select(c => new { c.Type, c.Value })
			});
		}

	}
}