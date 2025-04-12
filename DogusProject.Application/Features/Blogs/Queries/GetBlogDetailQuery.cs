using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Queries;

public record GetBlogDetailQuery(Guid BlogId) : IRequest<Result<BlogDetailDto>>;