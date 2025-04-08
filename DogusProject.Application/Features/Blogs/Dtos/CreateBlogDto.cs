namespace DogusProject.Application.Features.Blogs.Dtos;

public class CreateBlogDto
{
	public string Title { get; set; } = null!;
	public string Content { get; set; } = null!;
	public Guid CategoryId { get; set; }
	public List<Guid>? TagIds { get; set; }
	public string? ImagePath { get; set; }
}