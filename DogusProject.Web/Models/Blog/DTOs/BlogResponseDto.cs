using DogusProject.Domain.Enums;

namespace DogusProject.Web.Models.Blog.DTOs;

public class BlogResponseDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = default!;
	public string Content { get; set; } = default!;
	public DateTime CreatedAt { get; set; }
	public string? AuthorFullName { get; set; }
	public string? AuthorAvatarUrl { get; set; }
	public Guid CategoryId { get; set; }
	public string? CategoryName { get; set; }
	public List<string> Tags { get; set; } = new();
	public List<string> ImageUrls { get; set; } = new();
	public BlogStatus Status { get; set; } = default!;
}
