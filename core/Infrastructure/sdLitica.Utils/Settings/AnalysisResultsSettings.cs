namespace sdLitica.Utils.Settings
{
    public class AnalysisResultsSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string MongoDatabaseName { get; set; }
        public string MongoCollectionName { get; set; }

        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}