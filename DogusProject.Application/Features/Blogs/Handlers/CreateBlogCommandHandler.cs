using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Commands;
using DogusProject.Domain.Entities;
using DogusProject.Domain.Enums;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, Result<Guid>>
{
	private readonly IBlogRepository _blogRepository;
	private readonly IMapper _mapper;

	public CreateBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper)
	{
		_blogRepository = blogRepository;
		_mapper = mapper;
	}

	public async Task<Result<Guid>> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
	{
		var blog = _mapper.Map<Blog>(request.Blog);
		blog.Status = BlogStatus.Published;
		blog.CreatedAt = DateTime.UtcNow;
		blog.PublishedAt = DateTime.UtcNow;
		blog.UserId = request.UserId;

		if (request.Blog.TagIds is not null && request.Blog.TagIds.Any())
		{
			foreach (var tagId in request.Blog.TagIds)
			{
				blog.BlogTags.Add(new BlogTag
				{
					BlogId = blog.Id,
					TagId = tagId
				});
			}
		}

		await _blogRepository.AddAsync(blog);

		return Result<Guid>.SuccessResult(blog.Id, "Blog created successfully.");
	}
}