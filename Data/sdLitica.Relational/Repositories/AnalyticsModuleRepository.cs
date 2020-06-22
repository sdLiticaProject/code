using Microsoft.EntityFrameworkCore;
using sdLitica.Analytics;
using sdLitica.Relational.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sdLitica.Relational.Repositories
{
    /// <summary>
    /// This class provides data access operations for AnalyticsModule
    /// </summary>
    public class AnalyticsModuleRepository : RepositoryBase<AnalyticsModule>
    {
        public AnalyticsModuleRepository(MySqlDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Check whether element given by guid exists
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool Contains(Guid guid)
        {
            return Entity.Any(p => p.Id.Equals(guid));
        }

        /// <summary>
        /// Get all modules with related operations.
        /// </summary>
        /// <returns></returns>
        public IList<AnalyticsModule> GetAll()
        {
            return Entity.Include(e => e.ModulesOperations).ThenInclude(mo => mo.AnalyticsOperation).ToList();
        }

        /// <summary>
        /// Get a module given by id with related operations. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new AnalyticsModule GetById(Guid id)
        {
            return Entity.Include(e => e.ModulesOperations).ThenInclude(mo => mo.AnalyticsOperation).SingleOrDefault(e => e.Id == id);
        }

    }
}
