using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NewsLetter.Api.Entities;

public class ArticleConfigurations : IEntityTypeConfiguration<Article>
{
	public void Configure(EntityTypeBuilder<Article> builder)
	{
		builder
			.HasKey(x => x.Id);

		builder.Property(x => x.Title)
			.HasMaxLength(255);

		builder.Property(x => x.Content)
			.HasMaxLength(1000);
	}
}
