using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using DogusProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DogusProject.Persistence.Repositories;

public class BlogImageRepository(AppIdentityDbContext context) : EfRepository<BlogImage>(context), IBlogImageRepository
{
	public async Task<List<BlogImage>> GetImagesByBlogIdAsync(Guid blogId)
	{
		return await _context.BlogImages
			.Where(x => x.BlogId == blogId)
			.ToListAsync();
	}
}