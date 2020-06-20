using Microsoft.EntityFrameworkCore;
using sdLitica.Analytics;
using sdLitica.Relational.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sdLitica.Relational.Repositories
{
    /// <summary>
    /// Repository for metadata of operations. 
    /// </summary>
    public class AnalyticsOperationRepository : RepositoryBase<AnalyticsOperation>
    {
        public AnalyticsOperationRepository(MySqlDbContext context)
            : base(context)
        {
        }
        public bool Contains(Guid guid)
        {
            return Entity.Any(p => p.Id.Equals(guid));
        }
        public bool ContainsName(string name)
        {
            return Entity.Any(p => p.Name.Equals(name));
        }
        public AnalyticsOperation GetByName(string name)
        {
            return Entity.Where(p => p.Name.Equals(name)).Include(e => e.ModulesOperations).ThenInclude(mo => mo.AnalyticsModule).Single();
        }
        public IList<AnalyticsOperation> GetAll()
        {
            return Entity.ToList();
        }
    }
}
