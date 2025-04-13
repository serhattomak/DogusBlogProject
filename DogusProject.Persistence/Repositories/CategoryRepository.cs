using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using DogusProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DogusProject.Persistence.Repositories;

public class CategoryRepository : EfRepository<Category>, ICategoryRepository
{
	public CategoryRepository(AppIdentityDbContext context) : base(context) { }

	public async Task<List<Category>> GetAllWithBlogCountAsync()
	{
		return await _context.Categories
			.Include(c => c.Blogs)
			.ToListAsync();
	}
}