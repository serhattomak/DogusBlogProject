using System.Net.Http.Headers;

namespace DogusProject.Web.Handlers;

public class AuthorizationHandler : DelegatingHandler
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public AuthorizationHandler(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var token = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");

		if (!string.IsNullOrEmpty(token))
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}

		return await base.SendAsync(request, cancellationToken);
	}
}