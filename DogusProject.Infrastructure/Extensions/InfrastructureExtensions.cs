using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DogusProject.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		return services;
	}
}