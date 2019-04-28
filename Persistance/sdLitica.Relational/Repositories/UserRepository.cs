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
    /// <summary>
    /// This class provides data access operations of User entity
    /// </summary>
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        /// <summary>
        /// Creates this class for the provided context
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(MySqlDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// This method verifies if an user with provided email exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool Exists(string email)
        {
            return Entity.Any(u => u.Email.Equals(email));
        }

        /// <summary>
        /// Get an user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetByEmail(string email)
        {
            return Entity.SingleOrDefault(u => u.Email.Equals(email));
        }
    }
}
