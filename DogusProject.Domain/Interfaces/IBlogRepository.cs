using DogusProject.Domain.Common;
using DogusProject.Domain.Entities;

namespace DogusProject.Domain.Interfaces;

public interface IBlogRepository : IRepository<Blog>
{
	Task<List<Blog>> GetAllWithCategoryAsync();
	Task<Blog?> GetByIdWithDetailsAsync(Guid id);
	Task<(Blog Blog, string? AuthorFullName)> GetBlogWithAuthorAsync(Guid blogId);
	Task<List<Blog>> GetBlogsByCategoryIdAsync(Guid categoryId);
	Task<List<Blog>> GetBlogsByTagIdAsync(Guid tagId);
	Task<List<Blog>> GetBlogsByAuthorIdAsync(Guid userId);
	Task<Blog?> GetByIdWithCategoryAndTagsAsync(Guid id);

	Task<List<(Blog Blog, string? AuthorFullName, string? AuthorAvatarUrl)>>
		GetBlogsWithAuthorInfoByAuthorIdAsync(Guid authorId);
	Task<PagedResult<(Blog Blog, string? AuthorFullName, string? AuthorAvatarUrl)>> GetAllBlogsWithAuthorInfoAsync(int page, int pageSize);
	Task AddBlogTagsAsync(List<BlogTag> blogTags);
	Task RemoveBlogTagsAsync(Guid blogId);
}