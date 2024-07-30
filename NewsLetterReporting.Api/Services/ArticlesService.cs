using ErrorOr;
using NewsLetterReporting.Api.Contracts;
using NewsLetterReporting.Api.Errors;
using NewsLetterReporting.Api.Repositories;

namespace NewsLetterReporting.Api.Services;

public class ArticlesService(
	IArticlesRepository articlesRepository, 
	ILogger<ArticlesService> logger) 
	: IArticlesService
{
	public async Task<ErrorOr<ArticleResponse>> GetArticleById(Guid id, CancellationToken cancellationToken)
	{
		logger.LogInformation("Received request for service: {ServiceName} with request parameters: {RequestParameters}",
			nameof(GetArticleById),
			id);

		var articleWithEvents = await articlesRepository.GetArticleWithEventsByIdAsync(id, cancellationToken);
		if (articleWithEvents is null)
		{
			return ArticlesErrors.ArticleNotFound;
		}
		return articleWithEvents;
	}
}
