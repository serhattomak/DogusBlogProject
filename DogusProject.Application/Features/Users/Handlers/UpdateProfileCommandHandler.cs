using DogusProject.Application.Common;
using DogusProject.Application.Features.Users.Commands;
using DogusProject.Application.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Users.Handlers;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<string>>
{
	private readonly IUserService _userService;

	public UpdateProfileCommandHandler(IUserService userService)
	{
		_userService = userService;
	}

	public async Task<Result<string>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
	{
		return await _userService.UpdateProfileAsync(request.Dto, cancellationToken);
	}
}