namespace DogusProject.Web.Models.Tag.DTOs;

public class TagDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
}