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
    public interface IUserApiKeyRepository : IRepositoryBase<UserApiKey>
    {
        /// <summary>
        /// Get list of UserApiKey entities owned by User entity
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<UserApiKey> GetByUserId(Guid userId);

        /// <summary>
        /// Get an UserApiKey entiy using an api key value
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        Task<UserApiKey> GetByApiKeyAsync(string apiKey);
    }
}
