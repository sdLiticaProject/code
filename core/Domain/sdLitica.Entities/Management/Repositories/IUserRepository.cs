using System;
using System.Collections.Generic;
using System.Text;
using sdLitica.Entities.Abstractions;

namespace sdLitica.Entities.Management.Repositories
{
    /// <summary>
    /// This interface provides data access operations of User entity
    /// </summary>
    public interface IUserRepository : IRepositoryBase<User>
    {
        /// <summary>
        /// Get an user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        User GetByEmail(string email);
        /// <summary>
        /// This method verifies if an user with provided email exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True if the user exists</returns>
        bool Exists(string email);
    }
}
