using DogusProject.Domain.Interfaces;
using DogusProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DogusProject.Persistence.Repositories;

public class EfRepository<T>(AppIdentityDbContext context) : IRepository<T>
	where T : class
{
	protected readonly AppIdentityDbContext _context = context;
	private readonly DbSet<T> _entities = context.Set<T>();

	public async Task AddAsync(T entity)
	{
		await _entities.AddAsync(entity);
	}

	public void Delete(T entity)
	{
		_entities.Remove(entity);
	}

	public async Task<List<T>> GetAllAsync()
	{
		return await _entities.ToListAsync();
	}

	public async Task<T?> GetByIdAsync(Guid id)
	{
		return await _entities.FindAsync(id);
	}

	public void Update(T entity)
	{
		_entities.Update(entity);
	}

	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}
}