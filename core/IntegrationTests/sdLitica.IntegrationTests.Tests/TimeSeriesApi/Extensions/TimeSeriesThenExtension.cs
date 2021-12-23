using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions
{
	public static class TimeSeriesThenExtension
	{
		private static TimeSeriesApiFacade _facade;
		private static ProfileApiFacade _profileApiFacade;

		public static void Init(TimeSeriesApiFacade facade, ProfileApiFacade profileApiFacade)
		{
			_facade = facade;
			_profileApiFacade = profileApiFacade;
		}

		public static ThenStatement CreatedTimeSeriesIsEqualToExpected(this ThenStatement thenStatement,
			string testKey = null)
		{
			var expected =
				thenStatement.GetGivenData<TestTimeSeriesMetadataModel>(BddKeyConstants.TimeSeriesToCreate + testKey);

			TestTimeSeriesMetadataModel actual = null;

			try
			{
				actual =
					thenStatement.GetResultData<TestTimeSeriesMetadataModel>(
						BddKeyConstants.CreatedTimeSeries + testKey);
			}
			catch (KeyNotFoundException e)
			{
				Assert.Fail("Was unable to get created time-series due to request failure.");
			}

			var currentUser =
				_profileApiFacade.GetMe(thenStatement.GetGivenData<string>(BddKeyConstants.SessionTokenKey + testKey))
					.Map<TestUserModel>();

			Assert.That(actual, Is.EqualTo(expected));
			Assert.That(actual.DateCreated, Is.Not.Empty);
			Assert.That(actual.InfluxId, Is.Not.Empty);
			Assert.That(actual.UserId, Is.EqualTo(currentUser.Id));

			try
			{
				var creationTime = DateTimeOffset.Parse(actual.DateCreated);
				Assert.That(DateTimeOffset.UtcNow - creationTime, Is.LessThan(TimeSpan.FromHours(1)));
			}
			catch
			{
				Assert.Fail($"Could not parse createdDate {actual.DateCreated}");
			}

			return thenStatement;
		}

		public static ThenStatement TimeSeriesIsPresentInUserTimeSeries(this ThenStatement thenStatement,
			TestTimeSeriesMetadataModel expected,
			string testKey = null)
		{
			List<TestTimeSeriesMetadataModel> actual = null;

			try
			{
				actual =
					thenStatement.GetResultData<List<TestTimeSeriesMetadataModel>>(
						BddKeyConstants.UserTimeSeries + testKey);
			}
			catch (KeyNotFoundException e)
			{
				Assert.Fail("Was unable to get user time-series due to request failure.");
			}

			Assert.Contains(expected, actual);

			return thenStatement;
		}

		public static ThenStatement TimeSeriesIsNotPresentInUserTimeSeries(this ThenStatement thenStatement,
			TestTimeSeriesMetadataModel expected,
			string testKey = null)
		{
			List<TestTimeSeriesMetadataModel> actual = null;

			try
			{
				actual =
					thenStatement.GetResultData<List<TestTimeSeriesMetadataModel>>(
						BddKeyConstants.UserTimeSeries + testKey);
			}
			catch (KeyNotFoundException e)
			{
				Assert.Fail("Was unable to get user time-series due to request failure.");
			}

			Assert.That(!actual.Any(model => Equals(model, expected)), $"Expected {expected} to be removed from user time-series");

			return thenStatement;
		}

		public static ThenStatement TimeSeriesByIdIsEqualTo(this ThenStatement thenStatement,
			TestTimeSeriesMetadataModel expected,
			string testKey = null)
		{
			TestTimeSeriesMetadataModel actual = null;

			try
			{
				actual =
					thenStatement.GetResultData<TestTimeSeriesMetadataModel>(
						BddKeyConstants.UserTimeSeriesById + testKey);
			}
			catch (KeyNotFoundException e)
			{
				Assert.Fail("Was unable to get user time-series due to request failure.");
			}

			Assert.That(actual, Is.EqualTo(expected));

			return thenStatement;
		}
	}
}
