using Microsoft.AspNetCore.Http;

namespace DogusProject.Application.Interfaces;

public interface IFileService
{
	Task<(string FullPath, string RelativePath)> SaveFileAsync(IFormFile file, string folderName, CancellationToken cancellationToken);
}