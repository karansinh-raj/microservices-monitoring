using AutoMapper;
using NewsLetter.Api.Contracts;
using NewsLetter.Api.Entities;

namespace NewsLetter.Api.Mappers;

public class ArticleMappings : Profile
{
	public ArticleMappings()
	{
		CreateMap<CreateArticle, Article>();
		CreateMap<Article, ArticleResponse>();
	}
}
