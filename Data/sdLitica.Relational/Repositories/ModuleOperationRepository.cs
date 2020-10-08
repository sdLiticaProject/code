using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.Analytics;
using sdLitica.Relational.Context;

namespace sdLitica.Relational.Repositories
{
    /// <summary>
    /// This class provides data access operations for ModuleOperation entity.
    /// </summary>
    public class ModuleOperationRepository: RepositoryBase<AnalyticsModulesOperations>
    {
        public ModuleOperationRepository(MySqlDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Get all ModuleOperation's with related entities.
        /// </summary>
        /// <returns></returns>
        public IList<AnalyticsModulesOperations> GetAll()
        {
            return Entity.Include(e => e.AnalyticsOperation).Include(e => e.AnalyticsModule).ToList();
        }
    }
}
