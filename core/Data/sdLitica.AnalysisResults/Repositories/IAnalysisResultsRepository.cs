using System;
using sdLitica.AnalysisResults.Model;

namespace sdLitica.AnalysisResults.Repositories
{
    public interface IAnalysisResultsRepository
    {
        void Add(AnalysisResult result);

        AnalysisResult GetById(Guid id);
    }
}