using DogusProject.Domain.Entities;

namespace DogusProject.Domain.Interfaces;

public interface IBlogRepository : IRepository<Blog>
{
	Task<List<Blog>> GetAllWithCategoryAsync();
	Task<Blog?> GetByIdWithDetailsAsync(Guid id);
}