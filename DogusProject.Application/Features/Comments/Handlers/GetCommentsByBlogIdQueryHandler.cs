using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Comments.Dtos;
using DogusProject.Application.Features.Comments.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Comments.Handlers;

public class GetCommentsByBlogIdQueryHandler : IRequestHandler<GetCommentsByBlogIdQuery, Result<List<CommentDto>>>
{
	private readonly ICommentRepository _repository;
	private readonly IMapper _mapper;

	public GetCommentsByBlogIdQueryHandler(ICommentRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<List<CommentDto>>> Handle(GetCommentsByBlogIdQuery request, CancellationToken cancellationToken)
	{
		var comments = await _repository.GetByBlogIdAsync(request.BlogId);
		var dto = _mapper.Map<List<CommentDto>>(comments);

		return Result<List<CommentDto>>.SuccessResult(dto);
	}
}