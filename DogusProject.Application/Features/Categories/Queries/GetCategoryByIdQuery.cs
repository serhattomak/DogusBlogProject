using DogusProject.Application.Common;
using DogusProject.Application.Features.Categories.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Categories.Queries;

public class GetCategoryByIdQuery : IRequest<Result<CategoryDto>>
{
	public Guid Id { get; }

	public GetCategoryByIdQuery(Guid id)
	{
		Id = id;
	}
}