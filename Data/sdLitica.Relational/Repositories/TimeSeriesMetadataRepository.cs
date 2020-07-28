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

        /// <summary>
        /// Gets list of time-series metadata objects owned by user given by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TimeSeriesMetadata> GetByUserId(Guid userId)
        {
            return Entity.Include(e => e.User).Where(e => e.UserId.Equals(userId)).ToList();
        }

        /// <summary>
        /// Gets untracked time-series metadata object
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public TimeSeriesMetadata GetByIdReadonly(Guid guid)
        {
            return Entity.AsNoTracking().SingleOrDefault(e => e.Id == guid);
        }
    }
}
