namespace NewsLetter.Api.Contracts;

public record ArticleResponse(
	string Id,
	string Title,
	string Content,
	DateTime CreatedOnUtc);
