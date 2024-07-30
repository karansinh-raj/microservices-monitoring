using ErrorOr;
using NewsLetterReporting.Api.Contracts;

namespace NewsLetterReporting.Api.Services;

public interface IArticlesService
{
	Task<ErrorOr<ArticleResponse>> GetArticleById(Guid id, CancellationToken cancellationToken);
}
