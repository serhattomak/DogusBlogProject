using DogusProject.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DogusProject.API.Filters;

public class ValidationFilter : IActionFilter
{
	public void OnActionExecuting(ActionExecutingContext context)
	{
		if (!context.ModelState.IsValid)
		{
			var errors = context.ModelState
				.Where(x => x.Value.Errors.Count > 0)
				.SelectMany(e => e.Value.Errors)
				.Select(e => e.ErrorMessage)
				.ToList();

			context.Result = new BadRequestObjectResult(Result.FailureResult(errors));
		}
	}

	public void OnActionExecuted(ActionExecutedContext context) { }
}