namespace DogusProject.Application.Features.Comments.Dtos;

public class CreateCommentDto
{
	public Guid UserId { get; set; }
	public Guid BlogId { get; set; }
	public string Content { get; set; } = string.Empty;
}