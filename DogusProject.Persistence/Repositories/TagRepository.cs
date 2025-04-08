using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using DogusProject.Persistence.Context;

namespace DogusProject.Persistence.Repositories;

public class TagRepository : EfRepository<Tag>, ITagRepository
{
	public TagRepository(AppIdentityDbContext context) : base(context) { }
}