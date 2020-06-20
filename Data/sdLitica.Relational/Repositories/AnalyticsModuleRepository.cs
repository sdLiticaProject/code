using Microsoft.EntityFrameworkCore;
using sdLitica.Analytics;
using sdLitica.Relational.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sdLitica.Relational.Repositories
{
    public class AnalyticsModuleRepository : RepositoryBase<AnalyticsModule>
    {
        public AnalyticsModuleRepository(MySqlDbContext context)
            : base(context)
        {
        }

        public bool Contains(Guid guid)
        {
            return Entity.Any(p => p.Id.Equals(guid));
        }
        public IList<AnalyticsModule> GetAll()
        {
            return Entity.Include(e => e.ModulesOperations).ThenInclude(mo => mo.AnalyticsOperation).ToList();
        }
        public new AnalyticsModule GetById(Guid id)
        {
            return Entity.Include(e => e.ModulesOperations).SingleOrDefault(e => e.Id == id);
        }

    }
}
