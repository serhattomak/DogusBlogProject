using DogusProject.Web.Models.Category.DTOs;
using DogusProject.Web.Models.Tag.DTOs;

namespace DogusProject.Web.Models.Sidebar.ViewModels;

public class RightSidebarViewModel
{
	public List<CategoryDto> PopularCategories { get; set; } = new();
	public List<TagDto> PopularTags { get; set; } = new();
}