namespace DogusProject.Application.Features.Users.Dtos;

public class UserProfileDto
{
	public Guid UserId { get; set; }
	public string UserName { get; set; }
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string FullName => $"{FirstName} {LastName}";
	public string? Bio { get; set; }
	public string? Location { get; set; }
	public string? Website { get; set; }
	public string? AvatarUrl { get; set; }
	public DateTime CreatedAt { get; set; }
}