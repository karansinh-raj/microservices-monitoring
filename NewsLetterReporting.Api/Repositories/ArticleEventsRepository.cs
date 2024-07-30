using NewsLetterReporting.Api.Data;
using NewsLetterReporting.Api.Entities;

namespace NewsLetterReporting.Api.Repositories;

public class ArticleEventsRepository(
	NewsLetterReportingDbContext context) 
	: IArticleEventsRepository
{
	public Task<List<ArticleEvent>> GetArticleEventsByArticleId(Guid id)
	{
		return Task.FromResult(context.ArticleEvents.Where(articleEvent => articleEvent.ArticleId == id).ToList());
	}

	public void Add(ArticleEvent articleEvent)
	{
		context.ArticleEvents.Add(articleEvent);
	}

	public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
	{
		return await context.SaveChangesAsync(cancellationToken) > 0;
	}
}
