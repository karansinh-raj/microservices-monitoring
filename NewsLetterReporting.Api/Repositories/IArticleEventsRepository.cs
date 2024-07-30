using NewsLetterReporting.Api.Entities;

namespace NewsLetterReporting.Api.Repositories;

public interface IArticleEventsRepository
{
	Task<List<ArticleEvent>> GetArticleEventsByArticleId(Guid id);
	void Add(ArticleEvent articleEvent);
	Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
