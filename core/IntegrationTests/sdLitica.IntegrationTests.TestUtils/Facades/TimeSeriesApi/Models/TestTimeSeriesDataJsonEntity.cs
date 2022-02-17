using System.Collections.Generic;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Models;

namespace sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models
{
	public class TestTimeSeriesDataJsonEntity : TestModel
	{
		/// <summary>
		/// Timestamp
		/// </summary>
		public string Timestamp { get; set; }

		/// <summary>
		/// Timeseries list of fields
		/// </summary>
		public IDictionary<string, object> Fields { get; set; }

		/// <summary>
		/// Timeseries list of tags
		/// </summary>
		public IDictionary<string, string> Tags { get; set; }

		/// <summary>
		/// Method returns an entity Id to use for referencing
		/// entity in REST API
		/// </summary>
		/// <returns>enity refernce id for REST API</returns>
		public string getApiUrlPrefix()
		{
			return "";
		}

		public string GetApiUrlPrefix()
		{
			return ""; // throw new System.NotImplementedException();
		}

		/// <summary>
		/// List of API required links
		/// </summary>
		public List<TestEntityLinkModel> Links { get; set; }
	}
}
