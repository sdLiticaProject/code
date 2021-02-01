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
    public class UserAPIKeyRepository : RepositoryBase<UserAPIKey>
    {
        public UserAPIKeyRepository(MySqlDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get a User entity by an existing API key
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserAPIKey> GetByApiKeyAsync(string apiKey)
        {
            return await Entity.Include(ut => ut.User).SingleOrDefaultAsync(ut => ut.APIKey.Equals(apiKey));
        }

        /// <summary>
        /// Get list of API keys owned by a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<UserAPIKey> GetApiKeysByUser(User user)
        {
            return Entity.Where(t => t.UserId.Equals(user.Id)).ToList();
        }
    }
}
