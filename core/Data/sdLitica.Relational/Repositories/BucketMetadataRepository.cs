using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.TimeSeries;
using sdLitica.Relational.Context;

namespace sdLitica.Relational.Repositories
{
    public class BucketMetadataRepository : RepositoryBase<BucketMetadata>
    {
        public BucketMetadataRepository(MySqlDbContext context)
            : base(context)
        {

        }
        
        public List<BucketMetadata> GetByUserId(Guid userId)
        {
            return Entity.Include(e => e.User).Where(e => e.UserId.Equals(userId)).ToList();
        }
        
        public BucketMetadata GetByIdReadonly(Guid guid)
        {
            return Entity.AsNoTracking().SingleOrDefault(e => e.Id == guid);
        }
        
    }
}