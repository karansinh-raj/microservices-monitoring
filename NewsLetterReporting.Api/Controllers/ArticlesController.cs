using Microsoft.AspNetCore.Mvc;
using NewsLetterReporting.Api.Services;

namespace NewsLetterReporting.Api.Controllers;

[Route("api/[controller]")]
public class ArticlesController(
	IArticlesService articlesService) 
	: BaseController
{
	[HttpGet("{id:guid}", Name = nameof(GetArticleById))]
	public async Task<ActionResult> GetArticleById(Guid id, CancellationToken cancellationToken)
	{
		var articleResult = await articlesService.GetArticleById(id, cancellationToken);
		return articleResult.Match(
			Ok,
			Problem);
	}
}
