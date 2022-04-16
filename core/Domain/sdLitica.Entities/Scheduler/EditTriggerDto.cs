using System;

namespace sdLitica.Entities.Scheduler
{
	public class EditTriggerDto
	{
		public string CronSchedule { get; set; }
		public string FetchUrl { get; set; }
	}
}
