using sdLitica.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sdLitica.Entities.Management.Repositories
{
    public interface IUserTokenRepository : IRepositoryBase<UserToken>
    {
        UserToken GetByUser(User user);
        Task<UserToken> GetByTokenAsync(string token);
    }
}
