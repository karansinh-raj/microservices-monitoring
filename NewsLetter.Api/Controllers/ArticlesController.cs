using Microsoft.AspNetCore.Mvc;
using NewsLetter.Api.Contracts;
using NewsLetter.Api.Services;

namespace NewsLetter.Api.Controllers;

[Route("api/[controller]")]
public class ArticlesController(
	IArticlesService articlesService,
	NewsletterReportingApiClient newsletterReportingApiClient) 
	: BaseController
{
	[HttpGet(Name = nameof(GetArticles))]
	public async Task<ActionResult> GetArticles()
	{
		return Ok(
			await articlesService
			.GetArticlesAsync());
	}

	[HttpGet("{id:guid}", Name = nameof(GetArticleById))]
	public async Task<ActionResult> GetArticleById(
		Guid id, 
		CancellationToken cancellationToken)
	{
		var articleResult = await articlesService
			.GetArticleById(id, cancellationToken);

		return articleResult.Match(
			Ok,
			Problem);
	}

	[HttpPost(Name = nameof(CreateArticle))]
	public async Task<ActionResult> CreateArticle(
		CreateArticle createArticle, 
		CancellationToken cancellationToken)
	{
		var articleResult = await articlesService
			.CreateArticleAsync(createArticle, cancellationToken);

		return articleResult.Match(
			article => CreatedAtAction(
				nameof(GetArticleById), 
				new { Id = article.Id }, 
				article),
			Problem);
	}

	[HttpGet("events/{id:guid}", Name = nameof(GetArticleEventsById))]
	public async Task<ActionResult> GetArticleEventsById(
		Guid id, 
		CancellationToken cancellationToken)
	{
		var articleResult = await newsletterReportingApiClient
			.GetArticleEventsByIdAsync(id, cancellationToken);

		return articleResult.Match(
			Ok,
			Problem);
	}
}
