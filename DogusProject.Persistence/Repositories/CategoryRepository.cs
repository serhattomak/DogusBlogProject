using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using DogusProject.Persistence.Context;

namespace DogusProject.Persistence.Repositories;

public class CategoryRepository : EfRepository<Category>, ICategoryRepository
{
	public CategoryRepository(AppIdentityDbContext context) : base(context) { }
}