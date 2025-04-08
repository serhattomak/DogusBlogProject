namespace DogusProject.Application.Common;

public class Result<T>
{
	public bool Success { get; set; }
	public T? Data { get; set; }
	public List<string> Errors { get; set; } = new();
	public string? Message { get; set; }

	public static Result<T> SuccessResult(T data, string? message = null)
		=> new() { Success = true, Data = data, Message = message };

	public static Result<T> FailureResult(List<string> errors)
		=> new() { Success = false, Errors = errors };

	public static Result<T> FailureResult(string error)
		=> new() { Success = false, Errors = new List<string> { error } };
}

public class Result
{
	public bool Success { get; set; }
	public List<string> Errors { get; set; } = new();
	public string? Message { get; set; }

	public static Result SuccessResult(string? message = null)
		=> new() { Success = true, Message = message };

	public static Result FailureResult(List<string> errors)
		=> new() { Success = false, Errors = errors };

	public static Result FailureResult(string error)
		=> new() { Success = false, Errors = new List<string> { error } };
}