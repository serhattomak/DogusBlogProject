using DogusProject.Application.Common;
using DogusProject.Application.Features.BlogImages.Commands;
using DogusProject.Domain.Entities;
using DogusProject.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace DogusProject.Application.Features.BlogImages.Handlers;

public class CreateBlogImageCommandHandler : IRequestHandler<CreateBlogImageCommand, Result<string>>
{
	private readonly IBlogRepository _blogRepository;
	private readonly IBlogImageRepository _blogImageRepository;
	private readonly IWebHostEnvironment _env;

	public CreateBlogImageCommandHandler(IWebHostEnvironment env, IBlogRepository blogRepository, IBlogImageRepository blogImageRepository)
	{
		_env = env;
		_blogRepository = blogRepository;
		_blogImageRepository = blogImageRepository;
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
			var extension = Path.GetExtension(image.FileName);
			var fileName = $"{Guid.NewGuid()}{extension}";
			var folderPath = Path.Combine(_env.WebRootPath, "uploads", "blogs");

			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			var filePath = Path.Combine(folderPath, fileName);
			using var stream = new FileStream(filePath, FileMode.Create);
			await image.CopyToAsync(stream, cancellationToken);

			var relativePath = Path.Combine("uploads", "blogs", fileName).Replace("\\", "/");

			var blogImage = new BlogImage
			{
				BlogId = blog.Id,
				ImagePath = filePath,
				ImageUrl = "/" + relativePath
			};

			await _blogImageRepository.AddAsync(blogImage);
		}

		await _blogRepository.SaveChangesAsync();
		return Result<string>.SuccessResult("Images uploaded successfully.");
	}
}