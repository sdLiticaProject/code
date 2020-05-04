using sdLitica.Analytics;
using sdLitica.Relational.Context;
using System;

namespace sdLitica.Relational.Repositories
{
    /// <summary>
    /// Repository for metadata of operations. 
    /// </summary>
    public class OperationRepository: RepositoryBase<AnalyticsOperationRequest>
    {
        public OperationRepository(MySqlDbContext context)
            : base(context)
        {
        }


        /// <summary>
        /// Returns status of operation given by guid. -1 failed, 0 in progress, 1 complete
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>Enum value for status of operation</returns>
        public OperationStatus GetStatus(Guid guid)
        {
            AnalyticsOperationRequest operation = Entity.Find(guid);
            if (operation == null)
            {
                throw new ArgumentNullException("OperationRepository does not contain operation " + guid);
            }
            return operation.Status;
        }
    }
}
