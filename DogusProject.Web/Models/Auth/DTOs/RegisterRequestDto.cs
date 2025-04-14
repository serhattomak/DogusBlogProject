using System.ComponentModel.DataAnnotations;

namespace DogusProject.Web.Models.Auth.DTOs;

public class RegisterRequestDto
{
	[Required(ErrorMessage = "Ad boş bırakılamaz.")]
	public string FirstName { get; set; } = string.Empty;
	[Required(ErrorMessage = "Soyad boş bırakılamaz.")]
	public string LastName { get; set; } = string.Empty;
	[Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
	public string UserName { get; set; } = string.Empty;

	[Required(ErrorMessage = "Email boş bırakılamaz.")]
	[EmailAddress(ErrorMessage = "Geçerli bir email giriniz.")]
	public string Email { get; set; } = string.Empty;

	[Required(ErrorMessage = "Şifre boş bırakılamaz.")]
	[MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
	public string Password { get; set; } = string.Empty;

	[Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
	public string ConfirmPassword { get; set; } = string.Empty;
}