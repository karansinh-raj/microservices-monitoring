using AutoMapper;
using ErrorOr;
using MassTransit;
using NewsLetter.Api.Contracts;
using NewsLetter.Api.Entities;
using NewsLetter.Api.Errors;
using NewsLetter.Api.Repositories;
using NewsLetter.Shared;

namespace NewsLetter.Api.Services;

public class ArticlesService(
	IArticlesRepository articlesRepository,
	ILogger<ArticlesService> logger,
	IMapper mapper,
	IPublishEndpoint publishEndpoint) 
	: IArticlesService
{
	public async Task<ErrorOr<ArticleResponse>> CreateArticleAsync(
		CreateArticle createArticle, 
		CancellationToken cancellationToken)
	{
		logger.LogInformation("Received request for service: {ServiceName} with request data: {RequestData}",
			nameof(CreateArticleAsync),
			createArticle);

		var article = mapper.Map<Article>(createArticle);

		articlesRepository.CreateArticle(article, cancellationToken);
		await articlesRepository.SaveChangesAsync(cancellationToken);

		var articleCreatedEvent = new ArticleCreatedEvent
		{
			Id = article.Id,
			CreatedOnUtc = article.CreatedOnUtc
		};

		logger.LogInformation("Sending ArticleCreatedEvent to message queue at {DateTime}", DateTime.UtcNow);

		await publishEndpoint.Publish(articleCreatedEvent,cancellationToken);

		// Update the article create count matrices
		DiagnosticsConfig.ArticlesCreateCounter.Add(
			1,
			new KeyValuePair<string, object?>("articles.id", article.Id.ToString()),
			new KeyValuePair<string, object?>("articles.title", article.Title),
			new KeyValuePair<string, object?>("articles.created_date", article.CreatedOnUtc.ToShortDateString()));

		return mapper.Map<ArticleResponse>(article);
	}

	public async Task<List<ArticleResponse>> GetArticlesAsync()
	{
		logger.LogInformation("Received request for service: {ServiceName}",
			nameof(GetArticlesAsync));

		return mapper.Map<List<ArticleResponse>>(await articlesRepository.GetArticlesAsync());
	}

	public async Task<ErrorOr<ArticleResponse>> GetArticleById(Guid id, CancellationToken cancellationToken)
	{
		logger.LogInformation("Received request for service: {ServiceName} with request parameters: {RequestParameters}",
			nameof(GetArticleById),
			id);

		var article = await articlesRepository.GetArticleByIdAsync(id, cancellationToken);
		if (article is null)
		{
			return ArticlesErrors.ArticleNotFound;
		}

		var articleViewedEvent = new ArticleViewedEvent
		{
			Id = article.Id,
			ViewedOnUtc = DateTime.UtcNow
		};

		logger.LogInformation("Sending ArticleViewedEvent to message queue at {DateTime}", DateTime.UtcNow);

		await publishEndpoint.Publish(articleViewedEvent, cancellationToken);

		return mapper.Map<ArticleResponse>(article);
	}
}
