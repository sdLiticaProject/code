using System;

namespace sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi.Models
{
	public class TestCreateNewTriggerModel : TestModel
	{
		public Guid MetadataId { get; set; }
		public string CronSchedule { get; set; }
		public string FetchUrl { get; set; }
	}
}
