using DogusProject.Domain.Interfaces;
using DogusProject.Persistence.Context;
using DogusProject.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DogusProject.Persistence.Extensions;

public static class RepositoryExtensions
{
	public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AppIdentityDbContext>(options =>
			options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

		services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
		services.AddScoped<IBlogRepository, BlogRepository>();
		services.AddScoped<ICommentRepository, CommentRepository>();
		services.AddScoped<ICategoryRepository, CategoryRepository>();
		services.AddScoped<ITagRepository, TagRepository>();
		services.AddScoped<IBlogImageRepository, BlogImageRepository>();

		return services;
	}
}