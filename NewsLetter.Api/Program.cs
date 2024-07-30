using NewsLetter.Api.DependencyInjection;

// WebApplication builder
var builder = WebApplication.CreateBuilder(args);

// Configuration of logging
builder.ConfigureOpenTelemetryLogging();

// Dependency injection
builder.Services.AddApplicationServices(builder.Configuration);

// Build the application
var app = builder.Build();

// Database migrations
await app.ApplyDatabaseMigrations();

// Request pipeline
app.ConfigureRequestPipeline();

// Run the application
await app.RunAsync();
