using DogusProject.Domain.Common;

namespace DogusProject.Domain.Entities;

public class Tag : BaseEntity
{
	public string Name { get; set; } = null!;

	public ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();
}