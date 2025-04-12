using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using DogusProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DogusProject.Persistence.Repositories;

public class BlogRepository(AppIdentityDbContext context) : EfRepository<Blog>(context), IBlogRepository
{
	private readonly AppIdentityDbContext _context = context;

	public async Task AddAsync(Blog entity)
	{
		await _context.Blogs.AddAsync(entity);
		await _context.SaveChangesAsync();
	}
	public void Update(Blog entity)
	{
		_context.Blogs.Update(entity);
		_context.SaveChanges();
	}

	public void Delete(Blog entity)
	{
		_context.Blogs.Remove(entity);
		_context.SaveChanges();
	}

	public async Task<List<Blog>> GetAllAsync()
	{
		return await _context.Blogs.ToListAsync();
	}

	public async Task<List<Blog>> GetAllWithCategoryAsync()
	{
		return await _context.Blogs
			.Include(b => b.Category)
			.Include(b => b.BlogTags)
			.ThenInclude(bt => bt.Tag)
			.ToListAsync();
	}

	public async Task<Blog?> GetByIdAsync(Guid id)
	{
		return await _context.Blogs.FindAsync(id);
	}

	public async Task<Blog?> GetByIdWithDetailsAsync(Guid id)
	{
		return await _context.Blogs
			.Include(b => b.Category)
			.Include(b => b.BlogTags)
			.ThenInclude(bt => bt.Tag)
			.FirstOrDefaultAsync(b => b.Id == id);
	}

	public async Task<List<Blog>> GetBlogsByCategoryIdAsync(Guid categoryId)
	{
		return await _context.Blogs
			.Include(b => b.Category)
			.Include(b => b.BlogTags)
			.ThenInclude(bt => bt.Tag)
			.Where(b => b.CategoryId == categoryId)
			.ToListAsync();
	}

	public async Task<List<Blog>> GetBlogsByTagIdAsync(Guid tagId)
	{
		return await _context.Blogs
			.Include(b => b.Category)
			.Include(b => b.BlogTags)
			.ThenInclude(bt => bt.Tag)
			.Where(b => b.BlogTags.Any(bt => bt.TagId == tagId))
			.ToListAsync();
	}

	public async Task<List<Blog>> GetBlogsByAuthorIdAsync(Guid authorId)
	{
		return await _context.Blogs
			.Include(b => b.Category)
			.Include(b => b.BlogTags)
			.ThenInclude(bt => bt.Tag)
			.Where(b => b.UserId == authorId)
			.ToListAsync();
	}
	public async Task<Blog?> GetByIdWithCategoryAndTagsAsync(Guid id)
	{
		return await _context.Blogs
			.Include(b => b.Category)
			.Include(b => b.BlogTags)
			.ThenInclude(bt => bt.Tag)
			.FirstOrDefaultAsync(b => b.Id == id);
	}

	public async Task RemoveBlogTagsAsync(Guid blogId)
	{
		var tags = await _context.BlogTags
			.Where(bt => bt.BlogId == blogId)
			.ToListAsync();

		_context.BlogTags.RemoveRange(tags);
	}
}