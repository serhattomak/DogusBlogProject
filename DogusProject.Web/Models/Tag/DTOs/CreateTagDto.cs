using System.ComponentModel.DataAnnotations;

namespace DogusProject.Web.Models.Tag.DTOs;

public class CreateTagDto
{
	[Required(ErrorMessage = "Etiket adı zorunludur.")]
	public string Name { get; set; } = string.Empty;
}