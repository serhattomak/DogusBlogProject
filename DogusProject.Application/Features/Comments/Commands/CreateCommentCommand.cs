using DogusProject.Application.Common;
using DogusProject.Application.Features.Comments.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Comments.Commands;

public class CreateCommentCommand : IRequest<Result>
{
	public CreateCommentDto Dto { get; }

	public CreateCommentCommand(CreateCommentDto dto)
	{
		Dto = dto;
	}
}