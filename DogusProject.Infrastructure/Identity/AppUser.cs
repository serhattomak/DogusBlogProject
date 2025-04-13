using DogusProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DogusProject.Infrastructure.Identity;

public class AppUser : IdentityUser<Guid>
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string FullName => $"{FirstName} {LastName}";
	public string? Bio { get; set; }
	public string? AvatarPath { get; set; }
	public string? AvatarUrl { get; set; }
	public string? CoverUrl { get; set; }
	public string? Location { get; set; }
	public string? Website { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
	public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}