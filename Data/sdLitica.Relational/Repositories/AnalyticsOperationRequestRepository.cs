using sdLitica.Entities.Analytics;
using sdLitica.Relational.Context;
using System;

namespace sdLitica.Relational.Repositories
{
    /// <summary>
    /// This class provides data access operations for AnalyticsOperationRequest.
    /// </summary>
    public class AnalyticsOperationRequestRepository: RepositoryBase<UserAnalyticsOperation>
    {
        public AnalyticsOperationRequestRepository(MySqlDbContext context)
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
            UserAnalyticsOperation operation = Entity.Find(guid);
            if (operation == null)
            {
                throw new ArgumentNullException("OperationRequestRepository does not contain operation " + guid);
            }
            return operation.Status;
        }
    }
}
