using DogusProject.Domain.Common;

namespace DogusProject.Domain.Entities;

public class BlogImage : BaseEntity
{
	public Guid BlogId { get; set; }
	public Blog Blog { get; set; } = null!;
	public string ImagePath { get; set; } = null!;
	public string ImageUrl { get; set; } = null!;
}