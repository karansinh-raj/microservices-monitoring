using Microsoft.EntityFrameworkCore;
using NewsLetterReporting.Api.Contracts;
using NewsLetterReporting.Api.Data;
using NewsLetterReporting.Api.Entities;

namespace NewsLetterReporting.Api.Repositories;

public class ArticlesRepository(
	NewsLetterReportingDbContext context) 
	: IArticlesRepository
{
	public Task<List<Article>> GetArticlesAsync()
	{
		return Task.FromResult(context.Articles.ToList());
	}

	public async Task<Article?> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		return await context.Articles.FindAsync([id], cancellationToken);
	}

	public async Task<ArticleResponse?> GetArticleWithEventsByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		return await context
			.Articles
			.Where(article => article.Id == id)
			.Select(article => new ArticleResponse
			{
				Id = article.Id,
				CreatedOnUtc = article.CreatedOnUtc,
				PublishedOnUtc = article.PublishedOnUtc,
				Events = context
						.ArticleEvents
						.Where(articleEvent => articleEvent.ArticleId == article.Id)
						.Select(articleEvent => new ArticleEventResponse
						{
							Id = articleEvent.Id,
							CreatedOnUtc = articleEvent.CreatedOnUtc,
							EventType = articleEvent.EventType
						}).ToList()
			})
			.FirstOrDefaultAsync(cancellationToken);
	}

	public void Add(Article article)
	{
		context.Articles.Add(article);
	}

	public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
	{
		return await context.SaveChangesAsync(cancellationToken) > 0;
	}
}
