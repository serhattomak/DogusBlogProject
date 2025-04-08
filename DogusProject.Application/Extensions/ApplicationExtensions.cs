using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DogusProject.Application.Extensions;

public static class ApplicationExtensions
{
	public static IServiceCollection AddApplicationFeatures(this IServiceCollection services)
	{
		services.AddAutoMapper(Assembly.GetExecutingAssembly());
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return services;
	}
}