namespace NewsLetterReporting.Api.Contracts;

public class ArticleResponse
{
	public Guid Id { get; init; }
	public DateTime CreatedOnUtc { get; init; }
	public DateTime? PublishedOnUtc { get; init; }
	public List<ArticleEventResponse> Events { get; init; } = [];
}
