using DogusProject.Web.Models.Category.DTOs;
using DogusProject.Web.Models.Tag.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DogusProject.Web.Models.Blog.ViewModels;

public class CreateBlogViewModel
{
	[Required]
	public string Title { get; set; } = string.Empty;

	[Required]
	public string Content { get; set; } = string.Empty;

	[Required]
	public Guid CategoryId { get; set; }
	public List<TagDto> AvailableTags { get; set; } = new();
	public List<Guid> SelectedTagIds { get; set; } = new();
	public string? NewTags { get; set; }

	public List<CategoryDto> Categories { get; set; } = new();
	public List<TagDto> Tags { get; set; } = new();

	public List<IFormFile>? Images { get; set; } = new();
}