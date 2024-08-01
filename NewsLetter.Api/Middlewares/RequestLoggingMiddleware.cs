using System.Text;

namespace NewsLetter.Api.Middlewares;

public class RequestLoggingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<RequestLoggingMiddleware> _logger;

	public RequestLoggingMiddleware(
		RequestDelegate next, 
		ILogger<RequestLoggingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		context.Request.EnableBuffering();

		var request = context.Request;

		var body = request.Body;
		var buffer = new byte[Convert.ToInt32(request.ContentLength)];
		await body.ReadAsync(buffer, 0, buffer.Length);
		var requestBody = Encoding.UTF8.GetString(buffer);

		_logger.LogInformation(
			"Request: Method: {RequestMethod}, Path: {RequestPath}, Query: {RequestQuery} Body: {RequestBody}",
			request.Method,
			request.Path,
			request.QueryString.Value,
			requestBody);

		context.Request.Body.Position = 0;

		await _next(context);
	}
}
