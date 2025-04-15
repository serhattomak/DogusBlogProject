using DogusProject.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

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

		var hash = await GetFileHashAsync(file, cancellationToken);
		var extension = Path.GetExtension(file.FileName);
		var fileName = $"{hash}{extension}";

		var relativePath = Path.Combine("uploads", folderName, fileName).Replace("\\", "/");
		var fullPath = Path.Combine(_wwwrootPath, "uploads", folderName, fileName);

		var dirPath = Path.GetDirectoryName(fullPath);
		if (!Directory.Exists(dirPath))
			Directory.CreateDirectory(dirPath);

		if (!File.Exists(fullPath))
		{
			using var stream = new FileStream(fullPath, FileMode.Create);
			await file.CopyToAsync(stream, cancellationToken);
		}

		return (fullPath, "/" + relativePath);
	}

	private async Task<string> GetFileHashAsync(IFormFile file, CancellationToken cancellationToken)
	{
		using var sha256 = SHA256.Create();
		await using var stream = file.OpenReadStream();
		var hashBytes = await sha256.ComputeHashAsync(stream, cancellationToken);
		return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
	}
}