using DogusProject.Domain.Entities;

namespace DogusProject.Domain.Interfaces;

public interface ITagRepository : IRepository<Tag>
{
	Task<List<Tag>> GetAllWithBlogCountAsync();
}