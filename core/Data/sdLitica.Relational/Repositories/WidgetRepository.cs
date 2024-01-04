using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.TimeSeries;
using sdLitica.Relational.Context;

namespace sdLitica.Relational.Repositories
{
    public class WidgetRepository : RepositoryBase<WidgetMetadata>
    {
        public WidgetRepository(MySqlDbContext context)
            : base(context)
        {

        }
        
        public List<WidgetMetadata> GetByDashboardId(Guid dashboardId)
        {
            return Entity.Include(e => e.Dashboard).Where(e => e.DashboardId.Equals(dashboardId)).ToList();
        }
        
        public WidgetMetadata GetByIdReadonly(Guid guid)
        {
            return Entity.AsNoTracking().SingleOrDefault(e => e.Id == guid);
        }
    }
}