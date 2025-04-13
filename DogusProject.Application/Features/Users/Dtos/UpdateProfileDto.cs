namespace DogusProject.Application.Features.Users.Dtos;

public class UpdateProfileDto
{
	public Guid UserId { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string? Bio { get; set; }
	public string? Location { get; set; }
	public string? Website { get; set; }
}