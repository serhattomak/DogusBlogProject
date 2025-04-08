using DogusProject.API.Extensions;
using DogusProject.API.Filters;
using DogusProject.Application.Extensions;
using DogusProject.Application.Validators;
using DogusProject.Infrastructure.Extensions;
using DogusProject.Infrastructure.Identity;
using DogusProject.Infrastructure.Mappings;
using DogusProject.Persistence.Extensions;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
	options.Filters.Add<ValidationFilter>();
});

builder.Services.AddJwtBearerAuthentication(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddApplicationFeatures();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRepositories(builder.Configuration);

builder.Services.AddFluentValidationAuto();
builder.Services.AddExceptionHandling();
builder.Services.AddCorsPolicy();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocs();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(typeof(DogusProject.Application.Mappings.MappingProfile));

builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

var app = builder.Build();

app.UseGlobalExceptionHandler();
app.UseSwaggerDocs();
app.UseCors("Default");
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await IdentitySeeder.SeedDefaultRolesAsync(app.Services);

app.Run();