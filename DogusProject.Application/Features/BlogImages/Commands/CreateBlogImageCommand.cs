using DogusProject.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DogusProject.Application.Features.BlogImages.Commands;

public class CreateBlogImageCommand : IRequest<Result<string>>
{
	public Guid BlogId { get; set; }
	public List<IFormFile> Images { get; set; } = new();
}