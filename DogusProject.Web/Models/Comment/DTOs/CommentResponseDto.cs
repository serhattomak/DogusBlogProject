namespace DogusProject.Web.Models.Comment.DTOs;

public class CommentResponseDto
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public string Content { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public Guid BlogId { get; set; }
	public string BlogTitle { get; set; } = string.Empty;
	public string? AuthorFullName { get; set; }
}