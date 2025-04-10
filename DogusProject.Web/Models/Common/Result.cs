namespace DogusProject.Web.Models.Common;

public class Result<T>
{
	public bool Success { get; set; }
	public T? Data { get; set; }
	public List<string> Errors { get; set; } = new();
	public string? Message { get; set; }
}