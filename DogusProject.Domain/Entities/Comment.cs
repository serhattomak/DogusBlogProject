using DogusProject.Domain.Common;

namespace DogusProject.Domain.Entities;

public class Comment : BaseEntity
{
	public string Content { get; set; } = null!;
	public Guid UserId { get; set; }
	public Guid BlogId { get; set; }
	public Blog Blog { get; set; }
}