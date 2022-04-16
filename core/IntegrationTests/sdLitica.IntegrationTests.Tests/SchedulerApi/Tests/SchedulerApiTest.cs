using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.SchedulerApi.Extensions;
using sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi;
using sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi;

namespace sdLitica.IntegrationTests.Tests.SchedulerApi.Tests
{
	public class SchedulerApiTest : BaseWithDefaultUserTest
	{
		protected SchedulerApiFacade Facade;
		protected ProfileApiFacade ProfileFacade;
		protected TimeSeriesApiFacade TimeSeriesFacade;

		protected GivenStatement Given => new GivenStatement(Logger);

		[OneTimeSetUp]
		public void InitFacade()
		{
			// correct config
			Assert.AreEqual("user@sdcliud.io", Configuration.UserName);
			// todo bootstrap it later
			Facade = new SchedulerApiFacade(
				Logger,
				Configuration.SchedulerUrl);
			ProfileFacade = new ProfileApiFacade(
				Logger,
				Configuration.RootUrl);
			TimeSeriesFacade = new TimeSeriesApiFacade(
				Logger,
				Configuration.RootUrl);
			TimeSeriesWhenExtension.Init(TimeSeriesFacade);
			TimeSeriesThenExtension.Init(TimeSeriesFacade, ProfileFacade);
			SchedulerWhenExtension.Init(Facade);
			SchedulerThenExtension.Init(Facade, ProfileFacade);
		}
	}
}
