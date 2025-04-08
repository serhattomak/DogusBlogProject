﻿using DogusProject.Infrastructure.Identity;
using DogusProject.Infrastructure.Interfaces;
using DogusProject.Infrastructure.Services;
using DogusProject.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace DogusProject.API.Extensions;

public static class JwtAuthenticationExtensions
{
	public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddIdentity<AppUser, AppRole>()
			.AddEntityFrameworkStores<AppIdentityDbContext>()
			.AddDefaultTokenProviders();

		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["Jwt:Issuer"],
					ValidAudience = configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
					RoleClaimType = ClaimTypes.Role
				};
			});

		services.AddScoped<IAuthService, AuthService>();

		return services;
	}
}