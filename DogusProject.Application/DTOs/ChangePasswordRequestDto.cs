namespace DogusProject.Application.DTOs;

public class ChangePasswordRequestDto
{
	public Guid UserId { get; set; }
	public string CurrentPassword { get; set; } = string.Empty;
	public string NewPassword { get; set; } = string.Empty;
}