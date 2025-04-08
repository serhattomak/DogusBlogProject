using DogusProject.Application.Common;
using DogusProject.Application.Features.Blogs.Commands;
using DogusProject.Domain.Interfaces;
using MediatR;

namespace DogusProject.Application.Features.Blogs.Handlers;

public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand, Result>
{
	private readonly IBlogRepository _repository;

	public DeleteBlogCommandHandler(IBlogRepository repository)
	{
		_repository = repository;
	}

	public async Task<Result> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
	{
		var blog = await _repository.GetByIdAsync(request.BlogId);
		if (blog == null)
			return Result.FailureResult("Blog has not found.");

		if (blog.UserId != request.UserId)
			return Result.FailureResult("You can only delete your own blog.");

		_repository.Delete(blog);
		await _repository.SaveChangesAsync();

		return Result.SuccessResult("Blog deleted successfully.");
	}
}