using MassTransit;
using NewsLetter.Shared;
using NewsLetterReporting.Api.Entities;
using NewsLetterReporting.Api.Repositories;

namespace NewsLetterReporting.Api.Services;

public class ArticleCreatedConsumer(
	IArticlesRepository articlesRepository,
	ILogger<ArticleCreatedConsumer> logger)
	: IConsumer<ArticleCreatedEvent>
{
	public async Task Consume(ConsumeContext<ArticleCreatedEvent> context)
	{
		// Access the correlation ID from the message headers
		var correlationId = context.Headers.Get<string>("correlationId");
		//Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId);

		logger.LogInformation("Received request for ArticleCreatedConsumer with message data: {MessageData}",
			context.Message);

		var article = new Article
		{
			Id = context.Message.Id,
			CreatedOnUtc = context.Message.CreatedOnUtc
		};
		articlesRepository.Add(article);

		// CancellationToken.None because we don't want to cancel adding of an event coming from message bus
		await articlesRepository.SaveChangesAsync(CancellationToken.None);

		logger.LogInformation("Created a new article in ArticleCreatedConsumer with data: {ArticleData}",
			article);
	}
}
