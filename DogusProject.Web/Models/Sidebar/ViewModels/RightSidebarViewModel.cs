using DogusProject.Application.Features.Categories.Dtos;
using DogusProject.Application.Features.Tags.Dtos;

namespace DogusProject.Web.Models.Sidebar.ViewModels;

public class RightSidebarViewModel
{
	public List<PopularCategoryDto> PopularCategories { get; set; } = new();
	public List<PopularTagDto> PopularTags { get; set; } = new();
}