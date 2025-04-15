using DogusProject.Domain.Common;
using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using DogusProject.Infrastructure.Identity;
using DogusProject.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DogusProject.Persistence.Repositories;

public class BlogRepository(AppIdentityDbContext context, UserManager<AppUser> userManager) : EfRepository<Blog>(context), IBlogRepository
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
			.AsTracking()
			.FirstOrDefaultAsync(b => b.Id == id);
	}
	public async Task<List<(Blog Blog, string? AuthorFullName, string? AuthorAvatarUrl)>> GetBlogsWithAuthorInfoByAuthorIdAsync(Guid authorId)
	{
		var blogs = await _context.Blogs
			.Where(b => b.UserId == authorId)
			.Include(b => b.BlogImages)
			.Include(b => b.Category)
			.ToListAsync();

		var result = new List<(Blog, string?, string?)>();

		foreach (var blog in blogs)
		{
			var user = await _context.Users.FindAsync(blog.UserId);
			var fullName = user is null ? null : $"{user.FirstName} {user.LastName}";
			result.Add((blog, fullName, user?.AvatarUrl));
		}

		return result;
	}

	public async Task<PagedResult<(Blog Blog, string? AuthorFullName, string? AuthorAvatarUrl)>> GetAllBlogsWithAuthorInfoAsync(int page, int pageSize)
	{
		var query = _context.Blogs
			.Include(b => b.BlogImages)
			.Include(b => b.Category)
			.Include(bt => bt.BlogTags)
			.OrderByDescending(b => b.CreatedAt);

		var totalCount = await query.CountAsync();

		var blogs = await query
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();

		var result = new List<(Blog, string?, string?)>();

		foreach (var blog in blogs)
		{
			var user = await _context.Users.FindAsync(blog.UserId);
			var fullName = user is null ? null : $"{user.FirstName} {user.LastName}";
			var avatarUrl = user?.AvatarUrl;
			result.Add((blog, fullName, avatarUrl));
		}

		return new PagedResult<(Blog, string?, string?)>
		{
			Items = result,
			CurrentPage = page,
			PageSize = pageSize,
			TotalCount = totalCount
		};
	}

	public async Task<(Blog Blog, string? AuthorFullName)> GetBlogWithAuthorAsync(Guid blogId)
	{
		var blog = await _context.Blogs
			.Include(b => b.Category)
			.Include(b => b.BlogTags)
			.ThenInclude(bt => bt.Tag)
			.FirstOrDefaultAsync(b => b.Id == blogId);

		if (blog is null)
			return (null!, null);

		var user = await _context.Users.FindAsync(blog.UserId);
		var authorFullName = user is null ? null : $"{user.FirstName} {user.LastName}";

		return (blog, authorFullName);
	}

	public async Task AddBlogTagsAsync(List<BlogTag> blogTags)
	{
		if (blogTags.Any())
		{
			await _context.BlogTags.AddRangeAsync(blogTags);
		}
	}

	public async Task RemoveBlogTagsAsync(Guid blogId)
	{
		var tags = await _context.BlogTags
			.Where(bt => bt.BlogId == blogId)
			.ToListAsync();

		_context.BlogTags.RemoveRange(tags);
	}
}