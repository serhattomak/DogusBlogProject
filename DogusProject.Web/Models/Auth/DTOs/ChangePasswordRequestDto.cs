using System.ComponentModel.DataAnnotations;

namespace DogusProject.Web.Models.Auth.DTOs;

public class ChangePasswordRequestDto
{
	public Guid UserId { get; set; }
	[Required(ErrorMessage = "Mevcut şifre gereklidir.")]
	public string CurrentPassword { get; set; } = string.Empty;

	[Required(ErrorMessage = "Yeni şifre gereklidir.")]
	public string NewPassword { get; set; } = string.Empty;

	[Required(ErrorMessage = "Yeni şifre tekrar gereklidir.")]
	[Compare("NewPassword", ErrorMessage = "Şifreler uyuşmuyor.")]
	public string ConfirmNewPassword { get; set; } = string.Empty;
}