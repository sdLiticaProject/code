using sdLitica.Entities.Management;
using sdLitica.Entities.Management.Repositories;
using sdLitica.Relational.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Relational.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(MySqlDbContext context)
            : base(context)
        {

        }
    }
}
