using DogusProject.Domain.Entities;

namespace DogusProject.Domain.Interfaces;

public interface IBlogRepository : IRepository<Blog>
{
	Task<List<Blog>> GetAllWithCategoryAsync();
	Task<Blog?> GetByIdWithDetailsAsync(Guid id);
	Task<List<Blog>> GetBlogsByCategoryIdAsync(Guid categoryId);
	Task<List<Blog>> GetBlogsByTagIdAsync(Guid tagId);
	Task<List<Blog>> GetBlogsByAuthorIdAsync(Guid userId);
	Task RemoveBlogTagsAsync(Guid blogId);
}