using DogusProject.Domain.Entities;

namespace DogusProject.Domain.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
	Task<List<Comment>> GetByBlogIdAsync(Guid blogId);
	Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId);
	Task<List<(Comment Comment, string? AuthorFullName)>> GetCommentsWithAuthorsByBlogIdAsync(Guid blogId);
}