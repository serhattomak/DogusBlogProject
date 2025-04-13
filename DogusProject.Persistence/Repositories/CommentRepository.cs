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

	public async Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId)
	{
		return await _context.Comments
			.Include(c => c.Blog)
			.Where(c => c.UserId == userId)
			.ToListAsync();
	}

	public async Task<List<(Comment Comment, string? AuthorFullName)>> GetCommentsWithAuthorsByBlogIdAsync(Guid blogId)
	{
		var comments = await _context.Comments
			.Where(c => c.BlogId == blogId)
			.OrderByDescending(c => c.CreatedAt)
			.ToListAsync();

		var result = new List<(Comment, string?)>();

		foreach (var comment in comments)
		{
			var user = await _context.Users.FindAsync(comment.UserId);
			var fullName = user is null ? null : $"{user.FirstName} {user.LastName}";
			result.Add((comment, fullName));
		}

		return result;
	}
}