using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.Management;
using sdLitica.Entities.Management.Repositories;
using sdLitica.Relational.Context;

namespace sdLitica.Relational.Repositories
{
    /// <summary>
    /// This class provides data access operations of User API key entity
    /// </summary>
    public class UserApiKeyRepository : RepositoryBase<UserApiKey>, IUserApiKeyRepository
    {
        public UserApiKeyRepository(MySqlDbContext context)
            : base(context)
        {
            
        }

        
        /// <summary>
        /// Get a User entity by an existing API key
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserApiKey> GetByApiKeyAsync(string apiKey)
        {
            return await Entity.Include(ut => ut.User).SingleOrDefaultAsync(ut => ut.APIKey.Equals(apiKey));
        }

        /// <summary>
        /// Get list of API keys owned by a user identified by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserApiKey> GetByUserId(Guid userId)
        {
            IQueryable<UserApiKey> query = Entity.Where(t => t.UserId.Equals(userId));
            return query.ToList();
        }

    }
}
