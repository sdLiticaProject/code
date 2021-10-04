using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.TimeSeriesApi.Tests
{
	public class CreateTimeSeriesTest : BaseAuthorizedTimeSeriesTest
	{
		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestSmokeCreateNewTimeSeries()
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Description = TestStringHelper.RandomLatinString(),
				Name = TestStringHelper.RandomLatinString(),
			};

			Facade.PostCreateTimeSeries(Session, timeSeries).AssertSuccess();

			var result = Facade.GetAllTimeSeries(Session).AssertSuccess()
				.MapAndLog<IList<TestTimeSeriesMetadataModel>>();

			Assert.Contains(timeSeries.Name, result.Select(ts => ts.Name).ToList());
		}
	}
}
