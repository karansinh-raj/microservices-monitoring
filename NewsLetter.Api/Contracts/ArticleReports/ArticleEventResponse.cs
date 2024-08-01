namespace NewsLetter.Api.Contracts.ArticleReports;

public class ArticleEventResponse
{
	public Guid Id { get; init; }
	public DateTime CreatedOnUtc { get; init; }

	public ArticleEventType EventType { get; init; }
}
