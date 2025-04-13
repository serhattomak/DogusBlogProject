using DogusProject.Application.Common;
using DogusProject.Application.Features.BlogImages.Commands;
using DogusProject.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace DogusProject.Application.Features.BlogImages.Handlers;

public class DeleteBlogImageCommandHandler : IRequestHandler<DeleteBlogImageCommand, Result<string>>
{
	private readonly IBlogImageRepository _blogImageRepository;
	private readonly IWebHostEnvironment _env;

	public DeleteBlogImageCommandHandler(IBlogImageRepository blogImageRepository, IWebHostEnvironment env)
	{
		_blogImageRepository = blogImageRepository;
		_env = env;
	}

	public async Task<Result<string>> Handle(DeleteBlogImageCommand request, CancellationToken cancellationToken)
	{
		var image = await _blogImageRepository.GetByIdAsync(request.ImageId);
		if (image == null)
			return Result<string>.FailureResult("Image not found");

		if (File.Exists(image.ImagePath))
			File.Delete(image.ImagePath);

		_blogImageRepository.Delete(image);
		await _blogImageRepository.SaveChangesAsync();

		return Result<string>.SuccessResult("Image deleted successfully.");
	}
}