namespace DogusProject.Web.Models.Auth.DTOs;

public class AuthResponseDto
{
	public Guid UserId { get; set; }
	public string Token { get; set; } = string.Empty;
	public string UserName { get; set; } = string.Empty;
	public List<string> Roles { get; set; } = new();
}