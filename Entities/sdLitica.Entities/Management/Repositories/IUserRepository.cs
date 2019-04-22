using sdLitica.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Entities.Management.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        User GetByEmail(string email);
        bool Exists(string email);
    }
}
