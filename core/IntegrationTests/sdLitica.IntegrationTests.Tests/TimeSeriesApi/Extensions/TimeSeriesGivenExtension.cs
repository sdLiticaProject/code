using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;

namespace sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions
{
	public static class TimeSeriesGivenExtension
	{
		public static GivenStatement NewTimeSeries(this GivenStatement givenStatement,
			TestTimeSeriesMetadataModel timeSeries, string testKey = null)
		{
			givenStatement.GetStatementLogger()
				.Information($"[{{ContextStatement}}] Saving time-series to create {timeSeries}",
					givenStatement.GetType().Name);
			givenStatement.AddData(timeSeries, BddKeyConstants.TimeSeriesToCreate + testKey);
			return givenStatement;
		}
	}
}
