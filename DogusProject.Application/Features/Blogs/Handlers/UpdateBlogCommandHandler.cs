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
		var blog = await _repository.GetByIdWithCategoryAndTagsAsync(request.Blog.Id);
		if (blog == null)
			return Result.FailureResult("Blog has not found.");

		if (blog.UserId != request.UserId)
			return Result.FailureResult("You can only update your own blog.");

		_mapper.Map(request.Blog, blog);
		blog.UpdatedAt = DateTime.UtcNow;

		await _repository.RemoveBlogTagsAsync(blog.Id);

		var newTags = (request.Blog.TagIds ?? new List<Guid>())
			.Distinct()
			.Select(tagId => new BlogTag
			{
				BlogId = blog.Id,
				TagId = tagId
			}).ToList();

		await _repository.AddBlogTagsAsync(newTags);

		try
		{
			_repository.Update(blog);
			await _repository.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			return Result.FailureResult($"Update failed: {ex.Message}");
		}

		return Result.SuccessResult("Blog updated successfully.");
	}
}