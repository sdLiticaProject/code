using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi;

namespace sdLitica.IntegrationTests.Tests.TimeSeriesApi.Tests
{
	public class TimeSeriesApiTest : BaseWithDefaultUserTest
	{
		protected TimeSeriesApiFacade Facade;
		protected ProfileApiFacade ProfileFacade;

		protected GivenStatement Given => new GivenStatement(Logger);

		[OneTimeSetUp]
		public void InitFacade()
		{
			// correct config
			Assert.AreEqual("user@sdcliud.io", Configuration.UserName);
			// todo bootstrap it later
			Facade = new TimeSeriesApiFacade(
				Logger,
				Configuration.RootUrl);
			ProfileFacade = new ProfileApiFacade(
				Logger,
				Configuration.RootUrl);
			TimeSeriesWhenExtension.Init(Facade);
			TimeSeriesThenExtension.Init(Facade, ProfileFacade);
		}
	}
}
