using DogusProject.Application.Features.Categories.Dtos;
using DogusProject.Application.Features.Tags.Dtos;

namespace DogusProject.Application.Features.Blogs.Dtos;

public class BlogDetailDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Content { get; set; } = string.Empty;
	public CategoryDto Category { get; set; } = new();
	public List<TagDto> Tags { get; set; } = new();
	public string? ImagePath { get; set; }
	public List<string> ImageUrls { get; set; } = new();
	public Guid UserId { get; set; }
	public string? Author { get; set; }
	public DateTime? PublishedAt { get; set; }
	public DateTime CreatedAt { get; set; }
}