using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace NewsLetterReporting.Api.Entities;

public class ArticleEventConfigurations : IEntityTypeConfiguration<ArticleEvent>
{
	public void Configure(EntityTypeBuilder<ArticleEvent> builder)
	{
		builder.HasKey(x => x.Id);
	}
}
