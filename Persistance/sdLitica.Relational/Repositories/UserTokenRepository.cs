using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.Management;
using sdLitica.Entities.Management.Repositories;
using sdLitica.Relational.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdLitica.Relational.Repositories
{
    public class UserTokenRepository : RepositoryBase<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(MySqlDbContext context)
            : base(context)
        {

        }

        public async Task<UserToken> GetByTokenAsync(string token)
        {
            return await Entity.Include(ut => ut.User).SingleOrDefaultAsync(ut => ut.Token.Equals(token));
        }

        public UserToken GetByUser(User user)
        {
            return Entity.SingleOrDefault(t => t.UserId.Equals(user.Id));
        }
    }
}
