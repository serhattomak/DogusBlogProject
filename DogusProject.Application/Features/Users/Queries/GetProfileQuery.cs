using DogusProject.Application.Common;
using DogusProject.Application.Features.Users.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Users.Queries;

public class GetProfileQuery : IRequest<Result<UserProfileDto>>
{
	public Guid UserId { get; set; }

	public GetProfileQuery(Guid userId)
	{
		UserId = userId;
	}
}