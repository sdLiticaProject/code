using sdLitica.Entities.Management.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Services.Management
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;

        public UserService(IUserRepository userRepository, IUserTokenRepository userTokenRepository)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
        }


    }
}
