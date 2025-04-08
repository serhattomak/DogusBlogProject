using AutoMapper;
using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Commands;
using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommand, Result>
{
	private readonly IBlogRepository _repository;
	private readonly IMapper _mapper;

	public UpdateBlogCommandHandler(IBlogRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
	{
		var blog = await _repository.GetByIdAsync(request.Blog.Id);
		if (blog == null)
			return Result.FailureResult("Blog has not found.");

		if (blog.UserId != request.UserId)
			return Result.FailureResult("You can only update your own blog.");

		_mapper.Map(request.Blog, blog);
		blog.UpdatedAt = DateTime.UtcNow;

		blog.BlogTags.Clear();

		if (request.Blog.TagIds is not null && request.Blog.TagIds.Any())
		{
			blog.BlogTags = request.Blog.TagIds
				.Select(tagId => new BlogTag
				{
					BlogId = blog.Id,
					TagId = tagId
				}).ToList();
		}

		_repository.Update(blog);
		await _repository.SaveChangesAsync();

		return Result.SuccessResult("Blog updated successfully.");
	}
}