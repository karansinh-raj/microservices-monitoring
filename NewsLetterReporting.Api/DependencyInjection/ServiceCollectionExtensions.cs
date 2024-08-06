using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NewsLetterReporting.Api.Data;
using NewsLetterReporting.Api.Middlewares;
using NewsLetterReporting.Api.Repositories;
using NewsLetterReporting.Api.Services;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;

namespace NewsLetterReporting.Api.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationServices(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		// Add controllers
		services.AddControllers();

		// Add API explorer, and swagger
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		// Database context
		services.AddDbContext<NewsLetterReportingDbContext>(
			options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

		// Auto mapper
		services.AddAutoMapper(Assembly.GetExecutingAssembly());

		// FluentValidators
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.AddFluentValidationAutoValidation();

		// Repositories
		services.AddScoped<IArticlesRepository, ArticlesRepository>();
		services.AddScoped<IArticleEventsRepository, ArticleEventsRepository>();

		// Services
		services.AddScoped<IArticlesService, ArticlesService>();

		// MassTransit
		services.AddMassTransit(busConfigurator =>
		{
			busConfigurator.SetKebabCaseEndpointNameFormatter();

			busConfigurator.AddConsumer<ArticleCreatedConsumer>();
			busConfigurator.AddConsumer<ArticleViewedConsumer>();

			busConfigurator.UsingRabbitMq((context, configurator) =>
			{
				configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
				{
					host.Username(configuration["MessageBroker:Username"]!);
					host.Password(configuration["MessageBroker:Password"]!);
				});
				configurator.ConfigureEndpoints(context);
			});
		});

		// Global exception handler
		services.AddExceptionHandler<GlobalExceptionHandler>();
		services.AddProblemDetails();

		// Health checks for RabbitMQ
		services.AddHealthChecks()
		.AddRabbitMQ(options => options.ConnectionUri = new Uri(configuration["MessageBroker:Host"]!));

		// Add open telemetry services
		services.AddOpenTelemetryServices();

		return services;
	}

	public static IServiceCollection AddOpenTelemetryServices(
		this IServiceCollection services)
	{
		services
			.AddOpenTelemetry()
			.ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName))
			.WithMetrics(metrics =>
			{
				metrics
					.AddAspNetCoreInstrumentation()
					.AddHttpClientInstrumentation();

				metrics
					.AddOtlpExporter();
			})
			.WithTracing(traces =>
			{
				traces
					.AddAspNetCoreInstrumentation()
					.AddHttpClientInstrumentation()
					.AddEntityFrameworkCoreInstrumentation();

				traces
					.AddSource("MassTransit");

				traces
					.AddOtlpExporter();
			});

		return services;
	}
}
