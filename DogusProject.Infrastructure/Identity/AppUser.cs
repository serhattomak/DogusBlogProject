using DogusProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DogusProject.Infrastructure.Identity;

public class AppUser : IdentityUser<Guid>
{
	public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
	public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}