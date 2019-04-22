using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.Management;
using sdLitica.Entities.Management.Repositories;
using sdLitica.Relational.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sdLitica.Relational.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(MySqlDbContext context)
            : base(context)
        {

        }

        public bool Exists(string email)
        {
            return Entity.Any(u => u.Email.Equals(email));
        }

        public User GetByEmail(string email)
        {
            return Entity.SingleOrDefault(u => u.Email.Equals(email));
        }
    }
}
