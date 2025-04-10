using System.ComponentModel.DataAnnotations;

namespace DogusProject.Web.Models.Auth.DTOs;

public class UpdateUserInfoDto
{
	public Guid UserId { get; set; }
	[Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
	public string UserName { get; set; } = string.Empty;

	[Required(ErrorMessage = "Email boş bırakılamaz.")]
	[EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
	public string Email { get; set; } = string.Empty;
}