using DogusProject.Domain.Entities;

namespace DogusProject.Domain.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
	Task<List<Comment>> GetByBlogIdAsync(Guid blogId);
}