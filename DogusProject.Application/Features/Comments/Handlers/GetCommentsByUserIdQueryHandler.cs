using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Comments.Dtos;
using DogusProject.Application.Features.Comments.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Comments.Handlers;

public class GetCommentsByUserIdQueryHandler : IRequestHandler<GetCommentsByUserIdQuery, Result<List<CommentResponseDto>>>
{
	private readonly ICommentRepository _commentRepository;
	private readonly IMapper _mapper;

	public GetCommentsByUserIdQueryHandler(ICommentRepository commentRepository, IMapper mapper)
	{
		_commentRepository = commentRepository;
		_mapper = mapper;
	}

	public async Task<Result<List<CommentResponseDto>>> Handle(GetCommentsByUserIdQuery request, CancellationToken cancellationToken)
	{
		var comments = await _commentRepository.GetCommentsByUserIdAsync(request.UserId);
		var mapped = _mapper.Map<List<CommentResponseDto>>(comments);
		return Result<List<CommentResponseDto>>.SuccessResult(mapped);
	}
}