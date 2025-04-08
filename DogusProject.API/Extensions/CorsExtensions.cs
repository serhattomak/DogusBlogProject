namespace DogusProject.API.Extensions;

public static class CorsExtensions
{
	public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
	{
		services.AddCors(opt =>
		{
			opt.AddPolicy("Default", policy =>
			{
				policy.AllowAnyOrigin()
					.AllowAnyHeader()
					.AllowAnyMethod();
			});
		});

		return services;
	}
}