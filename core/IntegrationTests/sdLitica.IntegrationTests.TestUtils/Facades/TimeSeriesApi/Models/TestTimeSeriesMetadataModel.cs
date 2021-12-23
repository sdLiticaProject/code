namespace sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models
{
	public class TestTimeSeriesMetadataModel : TestModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string DateCreated { get; set; }
		public string DateModified { get; set; }
		public string UserId { get; set; }
		public string InfluxId { get; set; }

		public override bool Equals(object? obj)
		{
			return Equals(obj as TestTimeSeriesMetadataModel);
		}

		public bool Equals(TestTimeSeriesMetadataModel obj)
		{
			if (obj == null)
			{
				return false;
			}

			return Equals(obj.Description, Description) &&
			       Equals(obj.Name, Name);
		}
	}
}
