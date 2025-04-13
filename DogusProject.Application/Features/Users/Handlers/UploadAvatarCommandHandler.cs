using DogusProject.Application.Common;
using DogusProject.Application.Features.Users.Commands;
using DogusProject.Application.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Users.Handlers;

public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, Result<string>>
{
	private readonly IUserService _userService;

	public UploadAvatarCommandHandler(IUserService userService)
	{
		_userService = userService;
	}

	public async Task<Result<string>> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
	{
		return await _userService.UploadAvatarAsync(request.UserId, request.File, cancellationToken);
	}
}