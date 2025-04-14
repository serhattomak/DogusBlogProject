namespace DogusProject.API.Extensions;

public static class CorsExtensions
{
	public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
	{
		services.AddCors(opt =>
		{
			opt.AddPolicy("Default", policy =>
			{
				policy.WithOrigins("https://localhost:7099")
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials();
			});
		});

		return services;
	}
}