using DogusProject.Application.Common;
using DogusProject.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace DogusProject.API.Middlewares;

public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionMiddleware> _logger;

	public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";

		Result errorResponse;
		int statusCode;

		switch (exception)
		{
			case NotFoundException notFoundEx:
				statusCode = (int)HttpStatusCode.NotFound;
				errorResponse = Result.FailureResult(notFoundEx.Message);
				_logger.LogWarning(notFoundEx, "NotFoundException caught.");
				break;

			case BusinessException businessEx:
				statusCode = (int)HttpStatusCode.BadRequest;
				errorResponse = Result.FailureResult(businessEx.Message);
				_logger.LogWarning(businessEx, "BusinessException caught.");
				break;

			case UnauthorizedException unauthorizedEx:
				statusCode = (int)HttpStatusCode.Unauthorized;
				errorResponse = Result.FailureResult(unauthorizedEx.Message);
				_logger.LogWarning(unauthorizedEx, "Unauthorized action.");
				break;

			case ValidationException validationEx:
				statusCode = (int)HttpStatusCode.BadRequest;
				errorResponse = Result.FailureResult(validationEx.Errors);
				_logger.LogWarning(validationEx, "Validation errors.");
				break;

			default:
				statusCode = (int)HttpStatusCode.InternalServerError;
				errorResponse = Result.FailureResult("Internal server error.");
				_logger.LogError(exception, "Unhandled exception.");
				break;
		}

		context.Response.StatusCode = statusCode;

		var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		});

		await context.Response.WriteAsync(json);
	}
}
