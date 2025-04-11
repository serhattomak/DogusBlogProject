using DogusProject.Web.Models.Blog.DTOs;
using DogusProject.Web.Models.Common;

namespace DogusProject.Web.Models.Tag.ViewModels;

public class BlogsByTagViewModel
{
	public Guid TagId { get; set; }
	public string TagName { get; set; } = string.Empty;
	public PagedResult<BlogResponseDto> PagedBlogs { get; set; } = new();
}