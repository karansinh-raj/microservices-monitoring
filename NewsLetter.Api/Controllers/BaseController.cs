using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NewsLetter.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
	protected ActionResult Problem(List<Error> errors)
	{
		if (errors.Count is 0)
		{
			return Problem();
		}

		if (errors.All(error => error.Type == ErrorType.Validation))
		{
			return ValidationProblem(errors);
		}

		var firstError = errors[0];
		return Problem(firstError);
	}

	private ObjectResult Problem(Error firstError)
	{
		var statusCode = firstError.Type switch
		{
			ErrorType.Conflict => StatusCodes.Status409Conflict,
			ErrorType.Validation => StatusCodes.Status400BadRequest,
			ErrorType.NotFound => StatusCodes.Status404NotFound,
			ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
			_ => StatusCodes.Status500InternalServerError
		};
		return Problem(statusCode: statusCode, title: firstError.Description);
	}

	private ActionResult ValidationProblem(List<Error> errors)
	{
		var modelStateDictionary = new ModelStateDictionary();
		foreach (var error in errors)
		{
			modelStateDictionary.AddModelError(
				error.Code,
				error.Description);
		}
		return ValidationProblem(modelStateDictionary);
	}
}
