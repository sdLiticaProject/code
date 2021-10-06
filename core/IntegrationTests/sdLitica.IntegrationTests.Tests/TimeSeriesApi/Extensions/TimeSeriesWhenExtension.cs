using System;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions
{
	public static class TimeSeriesWhenExtension
	{
		private static TimeSeriesApiFacade _facade;

		public static void Init(TimeSeriesApiFacade facade)
		{
			_facade = facade;
		}

		public static WhenStatement CreateNewTimeSeriesRequestIsSend(this WhenStatement whenStatement, string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Getting current user", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);
			var newTimeSeries = whenStatement.GetGivenData<TestTimeSeriesMetadataModel>(BddKeyConstants.TimeSeriesToCreate + testKey);

			var response = _facade.PostCreateTimeSeries(session, newTimeSeries);
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);
			try
			{
				var createdTimeSeries = response.MapAndLog<TestTimeSeriesMetadataModel>();
				whenStatement.GetStatementLogger()
					.Information($"[{{ContextStatement}}] Got time series {createdTimeSeries}", whenStatement.GetType().Name);

				whenStatement.AddResultData(createdTimeSeries, BddKeyConstants.CreatedTimeSeries + testKey);
			}
			catch
			{
				whenStatement.GetStatementLogger()
					.Information("[{ContextStatement}] Could not find new time-series in response", whenStatement.GetType().Name);
			}

			return whenStatement;
		}
	}
}
