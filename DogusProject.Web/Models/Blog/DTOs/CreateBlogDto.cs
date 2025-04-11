using System.ComponentModel.DataAnnotations;

namespace DogusProject.Web.Models.Blog.DTOs;

public class CreateBlogDto
{
	[Required(ErrorMessage = "Başlık zorunludur.")]
	public string Title { get; set; } = string.Empty;

	[Required(ErrorMessage = "İçerik zorunludur.")]
	public string Content { get; set; } = string.Empty;

	[Required(ErrorMessage = "Kategori seçimi zorunludur.")]
	public Guid CategoryId { get; set; }

	public List<Guid> TagIds { get; set; } = new();

	public string? NewTagName { get; set; }
}