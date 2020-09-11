using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using sdLitica.Entities.Abstractions;

namespace sdLitica.Entities.Management.Repositories
{
    /// <summary>
    /// This interface provides data access operations of UserToken entity
    /// </summary>
    public interface IUserTokenRepository : IRepositoryBase<UserToken>
    {
        /// <summary>
        /// Get an UserToken by User entity
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        UserToken GetByUser(User user);
        /// <summary>
        /// Get an UserToken registry using an existing token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<UserToken> GetByTokenAsync(string token);
    }
}
