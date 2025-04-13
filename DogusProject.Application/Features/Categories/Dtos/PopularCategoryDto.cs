namespace DogusProject.Application.Features.Categories.Dtos;

public class PopularCategoryDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int BlogCount { get; set; }
}