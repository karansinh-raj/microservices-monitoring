using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NewsLetterReporting.Api.Entities;

public class ArticleConfigurations : IEntityTypeConfiguration<Article>
{
	public void Configure(EntityTypeBuilder<Article> builder)
	{
		builder.HasKey(x => x.Id);
	}
}
