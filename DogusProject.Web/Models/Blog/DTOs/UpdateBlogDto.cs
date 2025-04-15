namespace DogusProject.Web.Models.Blog.DTOs;

public class UpdateBlogDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Content { get; set; } = string.Empty;
	public Guid CategoryId { get; set; }
	public List<Guid> TagIds { get; set; } = new();
	public List<(Guid Id, string Url)> ExistingImages { get; set; } = new();
}