using DogusProject.Domain.Entities;

namespace DogusProject.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
	Task<List<Category>> GetAllWithBlogCountAsync();
}