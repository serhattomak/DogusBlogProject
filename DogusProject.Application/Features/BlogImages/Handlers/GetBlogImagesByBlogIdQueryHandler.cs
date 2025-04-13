using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.BlogImages.Dtos;
using DogusProject.Application.Features.BlogImages.Queries;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.BlogImages.Handlers;

public class GetBlogImagesByBlogIdQueryHandler : IRequestHandler<GetBlogImagesByBlogIdQuery, Result<List<BlogImageDto>>>
{
	private readonly IBlogImageRepository _blogImageRepository;
	private readonly IMapper _mapper;

	public GetBlogImagesByBlogIdQueryHandler(IBlogImageRepository blogImageRepository, IMapper mapper)
	{
		_blogImageRepository = blogImageRepository;
		_mapper = mapper;
	}

	public async Task<Result<List<BlogImageDto>>> Handle(GetBlogImagesByBlogIdQuery request, CancellationToken cancellationToken)
	{
		var images = await _blogImageRepository.GetImagesByBlogIdAsync(request.BlogId);
		var dto = _mapper.Map<List<BlogImageDto>>(images);
		return Result<List<BlogImageDto>>.SuccessResult(dto);
	}
}