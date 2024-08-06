using MassTransit;
using NewsLetter.Shared;
using NewsLetterReporting.Api.Entities;
using NewsLetterReporting.Api.Repositories;

namespace NewsLetterReporting.Api.Services;

public class ArticleViewedConsumer(
	IArticlesRepository articlesRepository,
	IArticleEventsRepository articleEventsRepository,
	ILogger<ArticleViewedConsumer> logger)
	: IConsumer<ArticleViewedEvent>
{
	public async Task Consume(ConsumeContext<ArticleViewedEvent> context)
	{
		logger.LogInformation("Received request for ArticleViewedConsumer with message data: {MessageData}",
			context.Message);

		var article = await articlesRepository.GetArticleByIdAsync(context.Message.Id, context.CancellationToken);
		if (article is null)
		{
			return;
		}

		var articleEvent = new ArticleEvent
		{
			Id = Guid.NewGuid(),
			ArticleId = article.Id,
			CreatedOnUtc = context.Message.ViewedOnUtc,
			EventType = ArticleEventType.ArticleViewed
		};

		articleEventsRepository.Add(articleEvent);

		// CancellationToken.None because we don't want to cancel adding of an event coming from message bus
		await articleEventsRepository.SaveChangesAsync(CancellationToken.None);

		logger.LogInformation("Created a new article view event in ArticleViewedConsumer with data: {ArticleEventData}",
			articleEvent);
	}
}
