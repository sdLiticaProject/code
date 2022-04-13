using System;
using Quartz;

namespace sdLitica.Triggers.Services
{
	public interface ITriggersService
	{
		void AddNewTrigger(Guid metadataId, string cronSchedule, string fetchUrl);
		void EditTrigger(Guid metadataId, string cronSchedule, string fetchUrl);
		void RemoveTrigger(Guid metadataId);
		void PauseJob(Guid metadataId);
		void ResumeJob(Guid metadataId);

		IJobDetail? GetJobInfo(Guid metadataId);
		ITrigger? GetTrigger(Guid metadataId);
	}
}
