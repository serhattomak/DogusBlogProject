using DogusProject.Application.Common;
using DogusProject.Application.Features.Tags.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Tags.Queries;

public class GetPopularTagsQuery : IRequest<Result<List<PopularTagDto>>> { }