using DogusProject.Web.Models.Blog.DTOs;
using DogusProject.Web.Models.Common;

namespace DogusProject.Web.Models.Category.ViewModels;

public class BlogsByCategoryViewModel
{
	public Guid CategoryId { get; set; }
	public string CategoryName { get; set; } = string.Empty;
	public PagedResult<BlogResponseDto> PagedBlogs { get; set; } = new();
}