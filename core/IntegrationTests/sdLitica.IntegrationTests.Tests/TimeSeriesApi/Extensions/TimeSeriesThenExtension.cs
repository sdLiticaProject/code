using System;
using System.Collections.Generic;
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
					.MapAndLog<TestUserModel>();

			Assert.That(actual.Description, Is.EqualTo(expected.Description));
			Assert.That(actual.Name, Is.EqualTo(expected.Name));
			Assert.That(actual.DateCreated, Is.Not.Empty);
			Assert.That(actual.DateModified, Is.EqualTo(actual.DateCreated));
			Assert.That(actual.InfluxId, Is.Not.Empty);
			Assert.That(actual.InfluxId, Is.Not.EqualTo(expected.InfluxId));
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
	}
}
