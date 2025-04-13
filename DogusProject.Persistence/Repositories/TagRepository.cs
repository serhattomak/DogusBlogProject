using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using DogusProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DogusProject.Persistence.Repositories;

public class TagRepository : EfRepository<Tag>, ITagRepository
{
	public TagRepository(AppIdentityDbContext context) : base(context) { }
	public async Task<List<Tag>> GetAllWithBlogCountAsync()
	{
		return await _context.Tags
			.Include(t => t.BlogTags)
			.ToListAsync();
	}
}