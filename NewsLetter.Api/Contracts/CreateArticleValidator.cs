using FluentValidation;

namespace NewsLetter.Api.Contracts;

public class CreateArticleValidator : AbstractValidator<CreateArticle>
{
	public CreateArticleValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty();

		RuleFor(x => x.Content)
			.NotEmpty();
	}
}
