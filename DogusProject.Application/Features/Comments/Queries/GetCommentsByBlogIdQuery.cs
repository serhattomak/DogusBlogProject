using DogusProject.Application.Common;
using DogusProject.Application.Features.Comments.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Comments.Queries;

public record GetCommentsByBlogIdQuery(Guid BlogId) : IRequest<Result<List<CommentResponseDto>>>;