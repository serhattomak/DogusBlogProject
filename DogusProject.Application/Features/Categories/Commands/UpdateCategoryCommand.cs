using DogusProject.Application.Common;
using DogusProject.Application.Features.Categories.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Categories.Commands;

public class UpdateCategoryCommand : IRequest<Result>
{
	public CategoryDto Category { get; set; }

	public UpdateCategoryCommand(CategoryDto category)
	{
		Category = category;
	}
}