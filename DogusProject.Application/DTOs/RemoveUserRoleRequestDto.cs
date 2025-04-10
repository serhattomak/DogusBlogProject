namespace DogusProject.Application.DTOs;

public class RemoveUserRoleRequestDto
{
	public Guid UserId { get; set; }
	public string RoleName { get; set; }
}