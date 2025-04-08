namespace DogusProject.Domain.Exceptions;

public class ValidationException : Exception
{
	public List<string> Errors { get; }

	public ValidationException(IEnumerable<string> errors)
		: base("Validation errors occured.")
	{
		Errors = errors.ToList();
	}
}