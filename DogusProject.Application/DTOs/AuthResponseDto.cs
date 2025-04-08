namespace DogusProject.Application.DTOs;

public class AuthResponseDto
{
	public string Token { get; set; }
	public string UserName { get; set; }
	public List<string> Roles { get; set; }
}