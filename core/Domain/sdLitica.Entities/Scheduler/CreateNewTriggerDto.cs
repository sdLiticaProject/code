using System;

namespace sdLitica.Entities.Scheduler
{
	public class CreateNewTriggerDto
	{
		public Guid MetadataId { get; set; }
		public string CronSchedule { get; set; }
	}
}
