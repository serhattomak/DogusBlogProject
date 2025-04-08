using DogusProject.Domain.Common;
using DogusProject.Domain.Enums;

namespace DogusProject.Domain.Entities;

public class Blog : BaseEntity
{
	public string Title { get; set; } = null!;
	public string Content { get; set; } = null!;
	public Guid UserId { get; set; }

	public Guid CategoryId { get; set; }
	public string? ImagePath { get; set; }
	public BlogStatus Status { get; set; } = BlogStatus.Draft;
	public DateTime? PublishedAt { get; set; }


	public Category Category { get; set; }
	public ICollection<Comment> Comments { get; set; }
	public ICollection<BlogTag> BlogTags { get; set; }
}