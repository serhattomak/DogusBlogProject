using DogusProject.Application.Interfaces;
using DogusProject.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DogusProject.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IFileService, FileService>();
		services.AddScoped<IUserService, UserService>();
		return services;
	}
}