namespace DogusProject.Web.Models.Auth.ViewModels;

public class EditProfileViewModel
{
	public Guid UserId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Bio { get; set; }
	public string? Location { get; set; }
	public string? Website { get; set; }
}