using ErrorOr;

namespace NewsLetter.Api.Errors;

public static class ArticlesErrors
{
	public static readonly Error ArticleNotFound =
		Error.NotFound("Articles.NotFound", "The article with the specified ID was not found.");
}
