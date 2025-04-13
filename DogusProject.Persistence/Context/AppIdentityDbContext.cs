using DogusProject.Domain.Entities;
using DogusProject.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DogusProject.Persistence.Context;

public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
	public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
	{

	}

	public DbSet<Blog> Blogs => Set<Blog>();
	public DbSet<BlogTag> BlogTags => Set<BlogTag>();
	public DbSet<BlogImage> BlogImages => Set<BlogImage>();
	public DbSet<Category> Categories => Set<Category>();
	public DbSet<Comment> Comments => Set<Comment>();
	public DbSet<Tag> Tags => Set<Tag>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.ApplyConfigurationsFromAssembly(typeof(AppIdentityDbContext).Assembly);
	}
}