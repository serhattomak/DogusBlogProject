using DogusProject.Application.Features.Categories.Dtos;
using DogusProject.Application.Features.Tags.Dtos;
using DogusProject.Web.Models.Common;
using DogusProject.Web.Models.Sidebar.ViewModels;
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
		var vm = new RightSidebarViewModel();

		var categoryResponse = await _client.GetAsync("category/popular");
		if (categoryResponse.IsSuccessStatusCode)
		{
			var result = await categoryResponse.Content.ReadFromJsonAsync<Result<List<PopularCategoryDto>>>();
			vm.PopularCategories = result?.Data?.Take(5).ToList() ?? new();
		}

		var tagResponse = await _client.GetAsync("tag/popular");
		if (tagResponse.IsSuccessStatusCode)
		{
			var result = await tagResponse.Content.ReadFromJsonAsync<Result<List<PopularTagDto>>>();
			vm.PopularTags = result?.Data?.Take(10).ToList() ?? new();
		}

		return View(vm);
	}
}