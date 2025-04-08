namespace DogusProject.Domain.Exceptions;

public class UnauthorizedException : Exception
{
	public UnauthorizedException(string message = "Unauthorized action.")
		: base(message)
	{
	}
}