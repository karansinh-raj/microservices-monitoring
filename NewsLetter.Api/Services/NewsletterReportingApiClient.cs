using ErrorOr;
using NewsLetter.Api.Contracts.ArticleReports;
using NewsLetter.Api.Errors;

namespace NewsLetter.Api.Services;

public class NewsletterReportingApiClient
{
	private readonly HttpClient _httpClient;

	public NewsletterReportingApiClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<ErrorOr<ArticleEventsResponse>> GetArticleEventsByIdAsync(
		Guid id, 
		CancellationToken ct)
	{
		var response = await _httpClient.GetAsync($"/api/articles/{id}", ct);

		if (response.IsSuccessStatusCode)
		{
			var articleEvents = await response.Content.ReadFromJsonAsync<ArticleEventsResponse>(cancellationToken: ct);
			if (articleEvents is null)
			{
				return ArticlesErrors.ArticleNotFound;
			}
			return articleEvents;
		}
		else
		{
			var problemDetails = await response.Content.ReadFromJsonAsync<Error>(cancellationToken: ct);
			return problemDetails;
		}
	}
}
