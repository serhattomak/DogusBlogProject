using System.ComponentModel.DataAnnotations;

namespace DogusProject.Web.Models.Category.DTOs;

public class CreateCategoryDto
{
	[Required(ErrorMessage = "Kategori adı boş bırakılamaz.")]
	public string Name { get; set; } = string.Empty;
}