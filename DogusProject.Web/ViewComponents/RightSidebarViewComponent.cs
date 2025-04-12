using DogusProject.Web.Models.Category.DTOs;
using DogusProject.Web.Models.Common;
using DogusProject.Web.Models.Sidebar.ViewModels;
using DogusProject.Web.Models.Tag.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DogusProject.Web.ViewComponents;

public class RightSidebarViewComponent : ViewComponent
{
	private readonly HttpClient _client;

	public RightSidebarViewComponent(IHttpClientFactory factory)
	{
		_client = factory.CreateClient("ApiClient");
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var categoriesResponse = await _client.GetAsync("category");
		var tagsResponse = await _client.GetAsync("tag");

		var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<Result<List<CategoryDto>>>();
		var tagsResult = await tagsResponse.Content.ReadFromJsonAsync<Result<List<TagDto>>>();

		var viewModel = new RightSidebarViewModel
		{
			PopularCategories = categoriesResult?.Data?.Take(5).OrderByDescending(x => x.CreatedAt).ToList() ?? new(),
			PopularTags = tagsResult?.Data?.Take(10).OrderByDescending(x => x.CreatedAt).ToList() ?? new()
		};

		return View(viewModel);
	}
}