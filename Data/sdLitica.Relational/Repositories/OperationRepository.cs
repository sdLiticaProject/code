using sdLitica.Analytics;
using sdLitica.Relational.Context;
using System;

namespace sdLitica.Relational.Repositories
{
    /// <summary>
    /// Repository for metadata of operations. 
    /// </summary>
    public class OperationRepository: RepositoryBase<AnalyticsOperation>
    {
        public OperationRepository(MySqlDbContext context)
            : base(context)
        {
        }


        /// <summary>
        /// Returns status of operation given by guid. -1 failed, 0 in progress, 1 complete
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>Status. -1 failed, 0 in progress, 1 complete</returns>
        public int GetStatus(Guid guid)
        {
            return Entity.Find(guid).Status;
        }
    }
}
