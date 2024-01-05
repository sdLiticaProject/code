using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.TimeSeries;
using sdLitica.Relational.Context;

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
        public List<TimeSeriesMetadata> GetByBucketId(Guid bucketId)
        {
            return Entity.Include(e => e.Bucket).Where(e => e.BucketId.Equals(bucketId)).ToList();
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
