using System.ComponentModel.DataAnnotations;

namespace DogusProject.Application.Features.Blogs.Dtos;

public class CreateBlogDto
{
	[Required]
	public string Title { get; set; } = null!;
	[Required]
	public string Content { get; set; } = null!;
	[Required]
	public Guid CategoryId { get; set; }
	public List<Guid>? TagIds { get; set; }
	public List<string> NewTags { get; set; } = new();
	public string? ImagePath { get; set; }
}