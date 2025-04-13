using DogusProject.Application.Common;
using DogusProject.Application.Features.Users.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Users.Commands;

public class UpdateProfileCommand : IRequest<Result<string>>
{
	public UpdateProfileDto Dto { get; set; }
	public UpdateProfileCommand(UpdateProfileDto dto)
	{
		Dto = dto;
	}

}