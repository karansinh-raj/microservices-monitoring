namespace NewsLetterReporting.Api.Entities;

public class Article
{
	public Guid Id { get; init; }
	public DateTime CreatedOnUtc { get; init; }
	public DateTime? PublishedOnUtc { get; init; }
}
