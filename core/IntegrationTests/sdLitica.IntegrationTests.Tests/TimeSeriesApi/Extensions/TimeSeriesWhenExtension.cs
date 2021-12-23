using System;
using System.Collections.Generic;
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

		public static WhenStatement CreateNewTimeSeriesRequestIsSend(this WhenStatement whenStatement,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Creating new time-series", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);
			var newTimeSeries =
				whenStatement.GetGivenData<TestTimeSeriesMetadataModel>(BddKeyConstants.TimeSeriesToCreate + testKey);

			var response = _facade.PostCreateTimeSeries(session, newTimeSeries);
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);
			try
			{
				var createdTimeSeries = response.Map<TestTimeSeriesMetadataModel>();
				whenStatement.GetStatementLogger()
					.Information($"[{{ContextStatement}}] Got time-series {createdTimeSeries}",
						whenStatement.GetType().Name);
				newTimeSeries.Id = createdTimeSeries.Id;
				newTimeSeries.InfluxId = createdTimeSeries.InfluxId;
				newTimeSeries.UserId = createdTimeSeries.UserId;
				whenStatement.AddResultData(createdTimeSeries, BddKeyConstants.CreatedTimeSeries + testKey);
			}
			catch
			{
				whenStatement.GetStatementLogger()
					.Information("[{ContextStatement}] Could not find new time-series in response",
						whenStatement.GetType().Name);
			}

			return whenStatement;
		}

		public static WhenStatement UpdateTimeSeriesRequestIsSend(this WhenStatement whenStatement,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Updating time-series", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);
			var timeSeries =
				whenStatement.GetGivenData<TestTimeSeriesMetadataModel>(BddKeyConstants.TimeSeriesToUpdate + testKey);

			var response = _facade.PostUpdateTimeSeries(session, timeSeries.Id, timeSeries);
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);

			return whenStatement;
		}

		public static WhenStatement RemoveTimeSeriesRequestIsSend(this WhenStatement whenStatement, string timeSeriesId,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Removing time-series", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);

			var response = _facade.DeleteTimeSeriesById(session, timeSeriesId);
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);

			return whenStatement;
		}

		public static WhenStatement UpdateGivenUpdateTimeSeriesIdsFromModel(this WhenStatement whenStatement, TestTimeSeriesMetadataModel model,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Updating time-series update params Ids", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);
			var timeSeries =
				whenStatement.GetGivenData<TestTimeSeriesMetadataModel>(BddKeyConstants.TimeSeriesToUpdate + testKey);

			timeSeries.Id = model.Id;
			timeSeries.InfluxId = model.InfluxId;
			timeSeries.UserId = model.UserId;

			return whenStatement;
		}

		public static WhenStatement UploadTimeSeriesDataRequestIsSend(this WhenStatement whenStatement, string fileContent,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Uploading .csv data to time-series", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);
			var newTimeSeries = whenStatement.GetTimeSeriesFromDatas(testKey);

			var response = _facade.PostUploadTimeSeries(session, newTimeSeries.Id, fileContent);
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);

			return whenStatement;
		}

		public static WhenStatement GetAllTimeSeriesRequestIsSend(this WhenStatement whenStatement,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Getting all user time-series", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);

			var response = _facade.GetAllTimeSeries(session);
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);

			try
			{
				var createdTimeSeries = response.Map<List<TestTimeSeriesMetadataModel>>();
				whenStatement.GetStatementLogger()
					.Information($"[{{ContextStatement}}] Got time-series {createdTimeSeries}",
						whenStatement.GetType().Name);

				whenStatement.AddResultData(createdTimeSeries, BddKeyConstants.UserTimeSeries + testKey);
			}
			catch
			{
				whenStatement.GetStatementLogger()
					.Information("[{ContextStatement}] Could not find list of time-series in response",
						whenStatement.GetType().Name);
			}
			return whenStatement;
		}

		public static WhenStatement GetTimeSeriesMetadataByIdRequestIsSend(this WhenStatement whenStatement, string timeSeriesId,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Getting user time-series by id", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);

			var response = _facade.GetTimeSeriesMetadataById(session, timeSeriesId);
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);

			try
			{
				var timeSeries = response.Map<TestTimeSeriesMetadataModel>();
				whenStatement.GetStatementLogger()
					.Information($"[{{ContextStatement}}] Got time-series {timeSeries}",
						whenStatement.GetType().Name);

				whenStatement.AddResultData(timeSeries, BddKeyConstants.UserTimeSeriesById + testKey);
			}
			catch
			{
				whenStatement.GetStatementLogger()
					.Information("[{ContextStatement}] Could not find time-series in response",
						whenStatement.GetType().Name);
			}
			return whenStatement;
		}

		private static TestTimeSeriesMetadataModel GetTimeSeriesFromDatas(this WhenStatement whenStatement,
			string testKey = null)
		{
			try
			{
				return whenStatement.GetResultData<TestTimeSeriesMetadataModel>(BddKeyConstants.CreatedTimeSeries +
					testKey);
			}
			catch (Exception e)
			{
				whenStatement.GetStatementLogger()
					.Information("[{ContextStatement}] Could not find time-series in response data",
						whenStatement.GetType().Name);
			}

			try
			{
				return whenStatement.GetResultData<TestTimeSeriesMetadataModel>(BddKeyConstants.LastHttpResponse +
					testKey);
			}
			catch (Exception e)
			{
				whenStatement.GetStatementLogger()
					.Information("[{ContextStatement}] Could not find time-series in last response data",
						whenStatement.GetType().Name);
				throw;
			}
		}
	}
}
