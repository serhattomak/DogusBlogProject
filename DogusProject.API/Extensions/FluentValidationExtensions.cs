using FluentValidation.AspNetCore;

namespace DogusProject.API.Extensions;

public static class FluentValidationExtensions
{
	public static IServiceCollection AddFluentValidationAuto(this IServiceCollection services)
	{
		services.AddFluentValidationAutoValidation();
		services.AddFluentValidationClientsideAdapters();
		return services;
	}
}