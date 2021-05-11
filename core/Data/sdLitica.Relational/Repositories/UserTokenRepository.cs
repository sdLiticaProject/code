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
    /// This class provides data access operations of UserToken entity
    /// </summary>
    public class UserTokenRepository : RepositoryBase<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(MySqlDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get an UserToken registry using an existing token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserToken> GetByTokenAsync(string token)
        {
            return await Entity.Include(ut => ut.User).SingleOrDefaultAsync(ut => ut.Token.Equals(token));
        }

        /// <summary>
        /// Get an UserToken by User entity
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserToken GetByUser(User user)
        {
            return Entity.SingleOrDefault(t => t.UserId.Equals(user.Id));
        }
    }
}
