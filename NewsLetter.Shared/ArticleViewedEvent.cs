namespace NewsLetter.Shared;

public class ArticleViewedEvent
{
	public Guid Id { get; set; }
	public DateTime ViewedOnUtc { get; set; }
}
