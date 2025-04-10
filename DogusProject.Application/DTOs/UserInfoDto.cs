namespace DogusProject.Application.DTOs;

public class UserInfoDto
{
	public Guid Id { get; set; } = default!;
	public string UserName { get; set; } = default!;
	public string Email { get; set; } = default!;
}