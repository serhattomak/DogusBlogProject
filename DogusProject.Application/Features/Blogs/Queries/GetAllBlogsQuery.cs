using DogusProject.Application.Common;
using DogusProject.Application.Common.Pagination;
using DogusProject.Application.Features.Blogs.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Queries;

public class GetAllBlogsQuery : PagedRequest, IRequest<Result<PagedResult<BlogResponseDto>>> { }