using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NewsLetter.Api.Data;
using NewsLetter.Api.Middlewares;
using NewsLetter.Api.Repositories;
using NewsLetter.Api.Services;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Net.Http.Headers;
using System.Reflection;

namespace NewsLetter.Api.DependencyInjection;

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
		services.AddDbContext<NewsLetterDbContext>(options => 
			options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

		// Auto mapper
		services.AddAutoMapper(Assembly.GetExecutingAssembly());

		// FluentValidators
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.AddFluentValidationAutoValidation();

		// Repositories
		services.AddScoped<IArticlesRepository, ArticlesRepository>();

		// Services
		services.AddScoped<IArticlesService, ArticlesService>();

		// MassTransit
		services.AddMassTransit(busConfigurator =>
		{
			busConfigurator.SetKebabCaseEndpointNameFormatter();
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

		// Global exception handler with problem details
		services.AddExceptionHandler<GlobalExceptionHandler>();
		services.AddProblemDetails();

		// Add health checks for RabbitMQ
		services.AddHealthChecks()
		.AddRabbitMQ(options => options.ConnectionUri = new Uri(configuration["MessageBroker:Host"]!));

		// Add open telemetry services
		services.AddOpenTelemetryServices();

		services.AddHttpClient<NewsletterReportingApiClient>(client =>
		{
			var baseUrl = configuration["NewsletterReportingApi:BaseUrl"];
			client.BaseAddress = new Uri(baseUrl!);
		});

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
					.AddMeter(DiagnosticsConfig.Meter.Name);

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
