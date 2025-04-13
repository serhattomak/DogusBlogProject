using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Domain.Common;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Queries;

public record GetAllBlogsWithAuthorQuery(int Page, int PageSize) : IRequest<PagedResult<BlogResponseDto>>;