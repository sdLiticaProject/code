using System;
using MongoDB.Driver;
using sdLitica.AnalysisResults.Model;
using sdLitica.AnalysisResults.Repositories;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;

namespace sdLitica.MongoConnector
{
    public class AnalysisResultsRepository: IAnalysisResultsRepository
    {
        private readonly AnalysisResultsSettings _settings;
        private readonly IMongoCollection<AnalysisResult> _mongoCollection;

        public AnalysisResultsRepository(IAppSettings settings, IMongoClient mongoClient)
        {
            _settings = settings.AnalysisResultsSettings;
            IMongoDatabase database = mongoClient.GetDatabase(_settings.MongoDatabaseName);
            _mongoCollection = database.GetCollection<AnalysisResult>(_settings.MongoCollectionName);
        }

        public void Add(AnalysisResult result) => _mongoCollection.InsertOne(result);

        public AnalysisResult GetById(Guid id) => _mongoCollection.FindSync(r => r.Id.Equals(id)).Single();
    }
}