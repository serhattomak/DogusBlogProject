using DogusProject.Application.Common;
using DogusProject.Application.Features.BlogImages.Commands;
using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using MediatR;
using IFileService = DogusProject.Application.Interfaces.IFileService;

namespace DogusProject.Application.Features.BlogImages.Handlers;

public class CreateBlogImageCommandHandler : IRequestHandler<CreateBlogImageCommand, Result<string>>
{
	private readonly IBlogRepository _blogRepository;
	private readonly IBlogImageRepository _blogImageRepository;
	private readonly IFileService _fileService;

	public CreateBlogImageCommandHandler(IBlogRepository blogRepository, IBlogImageRepository blogImageRepository, IFileService fileService)
	{
		_blogRepository = blogRepository;
		_blogImageRepository = blogImageRepository;
		_fileService = fileService;
	}

	public async Task<Result<string>> Handle(CreateBlogImageCommand request, CancellationToken cancellationToken)
	{
		var blog = await _blogRepository.GetByIdAsync(request.BlogId);
		if (blog == null)
			return Result<string>.FailureResult("Blog not found");

		if (request.Images.Count > 4)
			return Result<string>.FailureResult("Maximum 4 images allowed.");

		foreach (var image in request.Images)
		{
			var (fullPath, relativePath) = await _fileService.SaveFileAsync(image, "blogs", cancellationToken);

			var blogImage = new BlogImage
			{
				BlogId = blog.Id,
				ImagePath = fullPath,
				ImageUrl = relativePath
			};

			await _blogImageRepository.AddAsync(blogImage);
		}

		await _blogRepository.SaveChangesAsync();
		return Result<string>.SuccessResult("Images uploaded successfully.");
	}
}