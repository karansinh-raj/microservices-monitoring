using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace NewsLetter.Api;

public static class DiagnosticsConfig
{
	public const string ServiceName = "NewsLetter.Api";

	public static Meter Meter = new(ServiceName);
	public static Counter<int> ArticlesCreateCounter = Meter.CreateCounter<int>("article_create.count");
}
