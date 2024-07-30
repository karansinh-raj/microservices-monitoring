using NewsLetterReporting.Api.Entities;
using System.Collections.Specialized;
using System.Text.Json.Serialization;

namespace NewsLetterReporting.Api.Contracts;

public record ArticleEventResponse
{
	public Guid Id { get; init; }
	public DateTime CreatedOnUtc { get; init; }

	[JsonConverter(typeof(StringEnumerator))]
	public ArticleEventType EventType { get; init; }
}
