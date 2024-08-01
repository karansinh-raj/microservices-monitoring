using OpenTelemetry.Logs;

namespace NewsLetterReporting.Api.DependencyInjection;

public static class WebApplicationBuilderExtensions
{
	public static WebApplicationBuilder ConfigureOpenTelemetryLogging(
		this WebApplicationBuilder builder)
	{
		builder
			.Logging
			.AddOpenTelemetry(logging =>
			{
				logging.IncludeFormattedMessage = true;
				logging.IncludeScopes = true;

				logging
					.AddOtlpExporter();
			});

		return builder;
	}
}
