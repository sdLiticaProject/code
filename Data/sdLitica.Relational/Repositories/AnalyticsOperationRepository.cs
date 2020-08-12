using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.Analytics;
using sdLitica.Relational.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sdLitica.Relational.Repositories
{
    /// <summary>
    /// This class provides data access operations for AnalyticsOperation. 
    /// </summary>
    public class AnalyticsOperationRepository : RepositoryBase<AnalyticsOperation>
    {
        public AnalyticsOperationRepository(MySqlDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Check whether element given by guid exists. 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool Contains(Guid guid)
        {
            return Entity.Any(p => p.Id.Equals(guid));
        }

        /// <summary>
        /// Check whether element given by name exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsName(string name)
        {
            return Entity.Any(p => p.Name.Equals(name));
        }

        /// <summary>
        /// Get operation by name and its related modules. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AnalyticsOperation GetByName(string name)
        {
            return Entity.Where(p => p.Name.Equals(name)).Include(e => e.AnalyticsModulesOperations).ThenInclude(mo => mo.AnalyticsModule).SingleOrDefault();
        }

        /// <summary>
        /// Get all operations with its related modules. 
        /// </summary>
        /// <returns></returns>
        public IList<AnalyticsOperation> GetAll()
        {
            return Entity.Include(e => e.AnalyticsModulesOperations).ThenInclude(mo => mo.AnalyticsModule).ToList();
        }
    }
}
