namespace sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models
{
	public class TestTimeSeriesMetadataModel : TestModel
	{
		public string Id { get; set; }
		public string Name { get; set; }

		//todo remove after https://github.com/sdLiticaProject/code/issues/66 fix
		public string Description { get; set; } = string.Empty;
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

			//todo uncomment after https://github.com/sdLiticaProject/code/issues/66 fix
			return //Equals(obj.Description, Description) &&
			       Equals(obj.Name, Name);
		}
	}
}
