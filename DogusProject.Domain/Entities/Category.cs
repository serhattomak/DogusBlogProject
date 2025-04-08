using DogusProject.Domain.Common;

namespace DogusProject.Domain.Entities;

public class Category : BaseEntity
{
	public string Name { get; set; } = null!;

	public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}