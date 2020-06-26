using sdLitica.Entities.TimeSeries;
using sdLitica.Relational.Context;
using System;
using System.Collections.Generic;
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
    }
}
