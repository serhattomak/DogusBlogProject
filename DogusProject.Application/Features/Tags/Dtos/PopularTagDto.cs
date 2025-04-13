namespace DogusProject.Application.Features.Tags.Dtos;

public class PopularTagDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int BlogCount { get; set; }
}