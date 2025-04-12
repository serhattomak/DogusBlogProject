using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Web.Models.Comment.DTOs;
using DogusProject.Web.Models.Tag.DTOs;

namespace DogusProject.Web.Models.Blog.ViewModels;

public class BlogDetailViewModel
{
	public BlogDetailDto Blog { get; set; } = null!;
	public List<CommentResponseDto> Comments { get; set; } = new();
	public CreateCommentDto NewComment { get; set; } = new();
	public TagDto Tag { get; set; } = new();
}