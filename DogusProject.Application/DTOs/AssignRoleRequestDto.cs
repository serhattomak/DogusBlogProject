namespace DogusProject.Application.DTOs;

public class AssignRoleRequestDto
{
	public Guid UserId { get; set; }
	public string RoleName { get; set; }
}