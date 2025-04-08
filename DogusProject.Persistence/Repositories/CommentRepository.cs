using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using DogusProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DogusProject.Persistence.Repositories;

public class CommentRepository : EfRepository<Comment>, ICommentRepository
{
	private readonly AppIdentityDbContext _context;

	public CommentRepository(AppIdentityDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<List<Comment>> GetByBlogIdAsync(Guid blogId)
	{
		return await _context.Comments
			.Where(c => c.BlogId == blogId)
			.OrderByDescending(c => c.CreatedAt)
			.ToListAsync();
	}
}