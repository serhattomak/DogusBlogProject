using DogusProject.Domain.Enums;

namespace DogusProject.Web.Models.Blog.DTOs;

public class BlogResponseDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Content { get; set; } = string.Empty;
	public Guid UserId { get; set; }
	public string UserName { get; set; } = string.Empty;
	public Guid CategoryId { get; set; }
	public string CategoryName { get; set; } = string.Empty;
	public List<string> Tags { get; set; } = new();
	public DateTime CreatedAt { get; set; }
	public DateTime? PublishedAt { get; set; }
	public BlogStatus Status { get; set; }
}