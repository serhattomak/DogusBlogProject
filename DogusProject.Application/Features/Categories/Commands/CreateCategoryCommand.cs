using DogusProject.Application.Common;
using DogusProject.Application.Features.Categories.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Categories.Commands;

public class CreateCategoryCommand : IRequest<Result<Guid>>
{
	public CreateCategoryDto Category { get; set; }

	public CreateCategoryCommand(CreateCategoryDto category)
	{
		Category = category;
	}
}