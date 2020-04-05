using sdLitica.Analytics.Abstractions;
using sdLitica.Relational.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sdLitica.Relational.Repositories
{
    public class OperationRepository: RepositoryBase<AnalyticsOperation>
    {
        MySqlDbContext _context;
        public OperationRepository(MySqlDbContext context)
            : base(context)
        {
            _context = context;
        }


        /// <summary>
        /// Returns status of operation of time-series given by tsId. -1 failed, 0 in progress, 1 complete
        /// </summary>
        /// <param name="tsId"></param>
        /// <returns>Status. -1 failed, 0 in progress, 1 complete</returns>
        public int GetStatus(Guid guid)
        {
            return Entity.Find(guid).Status;
        }

        public void SetStatus(AnalyticsOperation op, int status)
        {
            _context.Update(op);
            _context.SaveChanges();
        }
    }
}
