using DogusProject.Application.Common;
using DogusProject.Application.Features.Users.Dtos;
using DogusProject.Application.Features.Users.Queries;
using DogusProject.Application.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Users.Handlers;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<UserProfileDto>>
{
	private readonly IUserService _userService;

	public GetProfileQueryHandler(IUserService userService)
	{
		_userService = userService;
	}

	public async Task<Result<UserProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
	{
		return await _userService.GetProfileAsync(request.UserId, cancellationToken);
	}
}