using NewsLetterReporting.Api.Contracts;
using NewsLetterReporting.Api.Entities;

namespace NewsLetterReporting.Api.Repositories;

public interface IArticlesRepository
{
	Task<List<Article>> GetArticlesAsync();
	Task<Article?> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<ArticleResponse?> GetArticleWithEventsByIdAsync(Guid id, CancellationToken cancellationToken);
	void Add(Article article);
	Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
