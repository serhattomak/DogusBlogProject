namespace DogusProject.Application.DTOs;

public class UserInfoDto
{
	public Guid Id { get; set; } = default!;
	public string UserName { get; set; } = default!;
	public string Email { get; set; } = default!;
	public string FirstName { get; set; } = default!;
	public string LastName { get; set; } = default!;
	public string? FullName { get; set; }
	public string? Bio { get; set; }
	public string? AvatarUrl { get; set; }
	public string? Location { get; set; }
	public string? Website { get; set; }
	public DateTime CreatedAt { get; set; }
}