namespace DogusProject.Application.Features.Blogs.Dtos;

public class BlogDetailDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Content { get; set; } = string.Empty;
	public string CategoryName { get; set; } = string.Empty;
	public List<string> Tags { get; set; } = new();
	public string? ImagePath { get; set; }
	public Guid UserId { get; set; }
	public string Author { get; set; } = string.Empty;
	public DateTime? PublishedAt { get; set; }
	public DateTime CreatedAt { get; set; }
}