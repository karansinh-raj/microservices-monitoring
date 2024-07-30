namespace NewsLetter.Shared;

public class ArticleCreatedEvent
{
	public Guid Id { get; set; }
	public DateTime CreatedOnUtc { get; set; }
}
