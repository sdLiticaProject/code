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
            return Entity.ToList();
        }
        

    }
}
