using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Queries;

public class GetAllBlogsQuery : IRequest<Result<List<BlogResponseDto>>> { }