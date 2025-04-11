using System.ComponentModel.DataAnnotations;

namespace DogusProject.Web.Models.Comment.DTOs;

public class CreateCommentDto
{
	public Guid BlogId { get; set; }

	[Required(ErrorMessage = "Yorum içeriği boş olamaz.")]
	public string Content { get; set; } = string.Empty;

	public Guid UserId { get; set; }
}