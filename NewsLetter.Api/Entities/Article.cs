namespace NewsLetter.Api.Entities;

public class Article
{
	public Guid Id { get; } = Guid.NewGuid();
	public string Title { get; init; } = string.Empty;
	public string Content { get; init; } = string.Empty;
	public DateTime CreatedOnUtc { get; } = DateTime.UtcNow;
}
