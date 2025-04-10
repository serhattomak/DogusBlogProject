using System.ComponentModel.DataAnnotations;

namespace DogusProject.Web.Models.Auth.DTOs;

public class LoginRequestDto
{
	[Required(ErrorMessage = "Email boş bırakılamaz.")]
	[EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
	[Display(Name = "Email")]
	public string Email { get; set; } = string.Empty;

	[Required(ErrorMessage = "Şifre boş bırakılamaz.")]
	[Display(Name = "Şifre")]
	public string Password { get; set; } = string.Empty;
}