using Microsoft.EntityFrameworkCore;
using NewsLetter.Api.Entities;
using System.Reflection;

namespace NewsLetter.Api.Data;

public class NewsLetterDbContext(DbContextOptions<NewsLetterDbContext> options) : DbContext(options)
{
	public DbSet<Article> Articles { get; init; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
