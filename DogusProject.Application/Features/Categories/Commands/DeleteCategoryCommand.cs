using DogusProject.Application.Common;
using MediatR;

namespace DogusProject.Application.Features.Categories.Commands;

public class DeleteCategoryCommand : IRequest<Result>
{
	public Guid Id { get; set; }

	public DeleteCategoryCommand(Guid id)
	{
		Id = id;
	}
}