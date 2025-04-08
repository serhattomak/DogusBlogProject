using DogusProject.API.Middlewares;

namespace DogusProject.API.Extensions;

public static class ExceptionHandlingExtensions
{
	public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
	{
		return services;
	}

	public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
	{
		return app.UseMiddleware<ExceptionMiddleware>();
	}
}