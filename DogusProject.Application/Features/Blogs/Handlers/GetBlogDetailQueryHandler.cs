using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Dtos;
using DogusProject.Application.Features.Blogs.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class GetBlogDetailQueryHandler : IRequestHandler<GetBlogDetailQuery, Result<BlogDetailDto>>
{
	private readonly IBlogRepository _blogRepository;
	private readonly IMapper _mapper;

	public GetBlogDetailQueryHandler(IBlogRepository blogRepository, IMapper mapper)
	{
		_blogRepository = blogRepository;
		_mapper = mapper;
	}

	public async Task<Result<BlogDetailDto>> Handle(GetBlogDetailQuery request, CancellationToken cancellationToken)
	{
		var blog = await _blogRepository.GetByIdWithCategoryAndTagsAsync(request.BlogId);
		if (blog == null)
			return Result<BlogDetailDto>.FailureResult("Blog bulunamadı.");

		var dto = _mapper.Map<BlogDetailDto>(blog);
		return Result<BlogDetailDto>.SuccessResult(dto);
	}
}