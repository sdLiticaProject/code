using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;

namespace sdLitica.IntegrationTests.Tests.TimeSeriesApi.Tests
{
	public class UploadTimeSeriesDataTests : BaseAuthorizedTimeSeriesTest
	{
		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestSmokeUploadTimeSeriesData()
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Description = TestStringHelper.RandomLatinString(),
				Name = TestStringHelper.RandomLatinString(),
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.UploadTimeSeriesDataRequestIsSend("2021-01-10 11:14,1\n2021-01-10 11:16,3")
				.Then
				.LastRequestSuccessful();
		}
	}
}
