using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.TimeSeries;
using sdLitica.Relational.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sdLitica.Relational.Repositories
{
    /// <summary>
    /// This class provides data access operations of TimeSeriesMetadata entity
    /// </summary>
    public class TimeSeriesMetadataRepository: RepositoryBase<TimeSeriesMetadata>
    {
        /// <summary>
        /// Creates this class for the provided context
        /// </summary>
        /// <param name="context"></param>
        public TimeSeriesMetadataRepository(MySqlDbContext context)
            : base(context)
        {

        }

        public List<TimeSeriesMetadata> GetByUserId(Guid userId)
        {
            return Entity.Include(e => e.User).Where(e => e.UserId.Equals(userId)).ToList();
        }
    }
}
