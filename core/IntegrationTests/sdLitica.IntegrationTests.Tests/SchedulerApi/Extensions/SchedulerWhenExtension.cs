﻿using System.Collections.Generic;
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
	}
}
