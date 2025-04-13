using DogusProject.Domain.Entities;

namespace DogusProject.Domain.Interfaces;

public interface IBlogImageRepository : IRepository<BlogImage>
{
	Task<List<BlogImage>> GetImagesByBlogIdAsync(Guid blogId);
}