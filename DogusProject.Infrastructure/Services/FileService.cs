using DogusProject.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DogusProject.Infrastructure.Services;

public class FileService : IFileService
{
	private readonly IWebHostEnvironment _env;
	private readonly string _wwwrootPath;

	public FileService(IWebHostEnvironment env)
	{
		_env = env;
		var projectRoot = Path.Combine(Directory.GetCurrentDirectory(), "..", "DogusProject.Web");
		_wwwrootPath = Path.Combine(projectRoot, "wwwroot");
	}

	public async Task<(string FullPath, string RelativePath)> SaveFileAsync(IFormFile file, string folderName, CancellationToken cancellationToken)
	{
		if (file == null || file.Length == 0)
			throw new ArgumentException("Invalid file");

		var extension = Path.GetExtension(file.FileName);
		var fileName = $"{Guid.NewGuid()}{extension}";
		var relativePath = Path.Combine("uploads", folderName, fileName).Replace("\\", "/");
		var fullPath = Path.Combine(_wwwrootPath, "uploads", folderName, fileName);

		var dirPath = Path.GetDirectoryName(fullPath);
		if (!Directory.Exists(dirPath))
			Directory.CreateDirectory(dirPath);

		using var stream = new FileStream(fullPath, FileMode.Create);
		await file.CopyToAsync(stream, cancellationToken);

		return (fullPath, "/" + relativePath);
	}
}