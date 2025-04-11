namespace DogusProject.Web.Models.Comment.DTOs;

public class CommentResponseDto
{
	public Guid Id { get; set; }
	public string Content { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public string BlogTitle { get; set; } = string.Empty;
}