using Microsoft.EntityFrameworkCore;
using NewsLetterReporting.Api.Entities;
using System.Reflection;

namespace NewsLetterReporting.Api.Data;

public class NewsLetterReportingDbContext(DbContextOptions<NewsLetterReportingDbContext> options) : DbContext(options)
{
	public DbSet<Article> Articles { get; set; } = null!;
	public DbSet<ArticleEvent> ArticleEvents { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
