using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Domain.Enums;
using DogusProject.Web.Models.Comment.DTOs;

namespace DogusProject.Web.Models.Auth.ViewModels;

public class UserProfileViewModel
{
	public string UserName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public List<string> Roles { get; set; } = new();
	public string? ImagePath { get; set; }
	public DateTime CreatedAt { get; set; }
	public BlogStatus Status { get; set; }

	public List<BlogResponseDto> Blogs { get; set; } = new();
	public List<CommentResponseDto> Comments { get; set; } = new();
}