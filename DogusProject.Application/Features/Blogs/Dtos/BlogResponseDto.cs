namespace DogusProject.Application.Features.Blogs.Dtos;

public class BlogResponseDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = null!;
	public string Content { get; set; } = null!;
	public string CategoryName { get; set; } = null!;
	public List<string> Tags { get; set; } = new();
	public string? ImagePath { get; set; }
	public string Author { get; set; } = null!;
	public DateTime? PublishedAt { get; set; }
}