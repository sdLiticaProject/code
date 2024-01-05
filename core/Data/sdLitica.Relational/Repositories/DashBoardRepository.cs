using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.TimeSeries;
using sdLitica.Relational.Context;

namespace sdLitica.Relational.Repositories
{
    public class DashBoardRepository : RepositoryBase<DashboardMetadata>
    {
        public DashBoardRepository(MySqlDbContext context)
            : base(context)
        {

        }
        
        public List<DashboardMetadata> GetByUserId(Guid userId)
        {
            return Entity.Include(e => e.User).Where(e => e.UserId.Equals(userId)).ToList();
        }
        
        public DashboardMetadata GetByIdReadonly(Guid guid)
        {
            return Entity.AsNoTracking().SingleOrDefault(e => e.Id == guid);
        }
    }
}