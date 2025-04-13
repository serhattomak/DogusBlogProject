using DogusProject.Web.Models.Blog.DTOs;
using DogusProject.Web.Models.Comment.DTOs;

namespace DogusProject.Web.Models.Auth.ViewModels;

public class UserProfileViewModel
{
	public string UserName { get; set; } = default!;
	public string Email { get; set; } = default!;
	public string FirstName { get; set; } = default!;
	public string LastName { get; set; } = default!;
	public string FullName { get; set; } = default!;
	public string? Bio { get; set; }
	public string? AvatarUrl { get; set; }
	public string? Location { get; set; }
	public string? Website { get; set; }
	public DateTime JoinedDate { get; set; }

	public int FollowingCount { get; set; } = 0;
	public int FollowerCount { get; set; } = 0;
	public int PostCount { get; set; } = 0;

	public List<BlogResponseDto> Blogs { get; set; } = new();
	public List<CommentResponseDto> Comments { get; set; } = new();
}