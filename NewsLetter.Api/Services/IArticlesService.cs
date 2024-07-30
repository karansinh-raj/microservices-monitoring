using ErrorOr;
using NewsLetter.Api.Contracts;

namespace NewsLetter.Api.Services;

public interface IArticlesService
{
	Task<ErrorOr<ArticleResponse>> CreateArticleAsync(CreateArticle createArticle, CancellationToken cancellationToken);
	Task<List<ArticleResponse>> GetArticlesAsync();
	Task<ErrorOr<ArticleResponse>> GetArticleById(Guid id, CancellationToken cancellationToken);
}
