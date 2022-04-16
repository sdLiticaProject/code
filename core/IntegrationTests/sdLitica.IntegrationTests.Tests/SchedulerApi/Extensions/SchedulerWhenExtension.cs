using System;
using System.Collections.Generic;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi;
using sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.SchedulerApi.Extensions
{
	public static class SchedulerWhenExtension
	{
		private static SchedulerApiFacade _facade;

		public static void Init(SchedulerApiFacade facade)
		{
			_facade = facade;
		}

		public static WhenStatement CreateNewTriggerRequestIsSend(this WhenStatement whenStatement,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Creating new trigger", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);
			var newTimeSeries =
				whenStatement.GetGivenData<TestCreateNewTriggerModel>(BddKeyConstants.TriggerToCreate + testKey);

			var response = _facade.PostCreateTrigger(session, newTimeSeries);
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);

			return whenStatement;
		}

		public static WhenStatement EditCreatedTriggerRequestIsSend(this WhenStatement whenStatement, TestEditTriggerModel editTrigger,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Editing trigger", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);
			var createdTrigger =
				whenStatement.GetGivenData<TestCreateNewTriggerModel>(BddKeyConstants.TriggerToCreate + testKey);

			var response = _facade.PutEditTrigger(session, createdTrigger.MetadataId.ToString(), editTrigger);
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);

			return whenStatement;
		}

		public static WhenStatement EditTriggerRequestIsSend(this WhenStatement whenStatement, string triggerId, TestEditTriggerModel editTrigger,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Editing trigger", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);

			var response = _facade.PutEditTrigger(session, triggerId, editTrigger);
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);

			return whenStatement;
		}

		public static WhenStatement DeleteCreatedTriggerRequestIsSend(this WhenStatement whenStatement,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Removing trigger", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);
			var createdTrigger =
				whenStatement.GetGivenData<TestCreateNewTriggerModel>(BddKeyConstants.TriggerToCreate + testKey);

			var response = _facade.DeleteRemoveTrigger(session, createdTrigger.MetadataId.ToString());
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);

			return whenStatement;
		}

		public static WhenStatement DeleteTriggerRequestIsSend(this WhenStatement whenStatement, string triggerId,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Removing trigger", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);

			var response = _facade.DeleteRemoveTrigger(session, triggerId);
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);

			return whenStatement;
		}

		public static WhenStatement GetCurrentTriggersCount(this WhenStatement whenStatement,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Creating new trigger", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);

			var response = _facade.GetAllTriggers(session);
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);
			try
			{
				var triggers = response.Map<List<TestGetTriggerModel>>();
				whenStatement.GetStatementLogger()
					.Information("[{ContextStatement}] Got triggers {Triggers}",
						whenStatement.GetType().Name, triggers);
				whenStatement.AddResultData(triggers.Count, BddKeyConstants.TriggersCount + testKey);
			}
			catch
			{
				whenStatement.GetStatementLogger()
					.Information("[{ContextStatement}] Could not find triggers in response",
						whenStatement.GetType().Name);
			}
			return whenStatement;
		}

		public static WhenStatement PauseCreatedTrigger(this WhenStatement whenStatement,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Pausing trigger", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);
			var trigger = whenStatement.GetGivenData<TestCreateNewTriggerModel>(BddKeyConstants.TriggerToCreate + testKey);
			var response = _facade.PostPauseTrigger(session, trigger.MetadataId.ToString());
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);
			return whenStatement;
		}

		public static WhenStatement PauseTrigger(this WhenStatement whenStatement, Guid triggerId,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Pausing trigger", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);
			var response = _facade.PostPauseTrigger(session, triggerId.ToString());
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);
			return whenStatement;
		}

		public static WhenStatement ResumeCreatedTrigger(this WhenStatement whenStatement,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Resuming trigger", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);
			var trigger = whenStatement.GetGivenData<TestCreateNewTriggerModel>(BddKeyConstants.TriggerToCreate + testKey);
			var response = _facade.PostResumeTrigger(session, trigger.MetadataId.ToString());
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);
			return whenStatement;
		}

		public static WhenStatement ResumeTrigger(this WhenStatement whenStatement, Guid triggerId,
			string testKey = null)
		{
			whenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Resuming trigger", whenStatement.GetType().Name);

			var session = whenStatement.GetSessionFromData(testKey);
			var response = _facade.PostResumeTrigger(session, triggerId.ToString());
			whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);
			return whenStatement;
		}
	}
}
