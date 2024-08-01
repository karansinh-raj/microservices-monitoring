using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using NewsLetterReporting.Api.Data;

namespace NewsLetterReporting.Api.DependencyInjection;

public static class WebApplicationExtensions
{
	public static async Task<WebApplication> ApplyDatabaseMigrations(
		this WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<NewsLetterReportingDbContext>();

		await dbContext.Database.MigrateAsync();

		return app;
	}

	public static WebApplication ConfigureRequestPipeline(
		this WebApplication app)
	{
		// Swagger
		app.UseSwagger();
		app.UseSwaggerUI();

		// Global exception handler
		app.UseExceptionHandler();

		// Authentication and authorization
		app.UseAuthentication();
		app.UseAuthorization();

		// Map controllers
		app.MapControllers();

		// health checks
		app.MapHealthChecks(
			"/health",
			new HealthCheckOptions
			{
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			});

		return app;
	}
}
