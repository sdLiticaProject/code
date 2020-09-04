using sdLitica.Entities.Analytics;
using sdLitica.Relational.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        /// Returns status of operation given by guid.
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

        /// <summary>
        /// Returns all user's analytical operations
        /// </summary>
        /// <returns></returns>
        public List<UserAnalyticsOperation> GetAll()
        {
            return Entity.ToList();
        }
    }
}
