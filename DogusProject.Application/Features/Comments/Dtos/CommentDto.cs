namespace DogusProject.Application.Features.Comments.Dtos;

public class CommentDto
{
	public Guid Id { get; set; }
	public string Content { get; set; } = null!;
	public DateTime CreatedAt { get; set; }
}