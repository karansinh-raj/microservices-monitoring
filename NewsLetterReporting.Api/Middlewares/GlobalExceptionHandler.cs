using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace NewsLetterReporting.Api.Middlewares;

public class GlobalExceptionHandler(
	ILogger<GlobalExceptionHandler> logger) 
	: IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		logger.LogError(exception, "An exception has been occurred.");

		var problemDetails = new ProblemDetails
		{
			Status = StatusCodes.Status500InternalServerError,
			Title = "An exception has been occurred.",
			Detail = exception.Message
		};

		httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

		return true;
	}
}
