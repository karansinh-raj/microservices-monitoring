using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using NewsLetter.Api.Data;

namespace NewsLetter.Api.DependencyInjection;

public static class WebApplicationExtensions
{
	public static async Task<WebApplication> ApplyDatabaseMigrations(
		this WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<NewsLetterDbContext>();

		await dbContext.Database.MigrateAsync();

		return app;
	}

	public static WebApplication ConfigureRequestPipeline(
		this WebApplication app)
	{
		// Swagger
		app.UseSwagger();
		app.UseSwaggerUI();

		// Https redirection
		app.UseHttpsRedirection();

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
