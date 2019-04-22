using sdLitica.Entities.Management;
using sdLitica.Entities.Management.Repositories;
using sdLitica.Services.Management.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public void CreateUser(string firstName, string lastName, string email, string plainPassword)
        {
            if (_userRepository.Exists(email))
                throw new EmailUsedException(email);
            
            var user = User.Create(firstName, lastName, email, plainPassword);            
            _userRepository.Add(user);
            _userRepository.SaveChanges();
        }

        public async Task<UserToken> GetNewTokenAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var userToken = _userTokenRepository.GetByUser(user);
            if (userToken == null)
            {
                userToken = UserToken.Create(user);
                _userTokenRepository.Add(userToken);
            }
            else
            {
                userToken.CreateToken();
                _userTokenRepository.Update(userToken);
            }
            await _userTokenRepository.SaveChangesAsync();
            return userToken;
        }

        public async Task<UserToken> ShiftTokenAsync(UserToken userToken)
        {
            if (userToken == null) throw new ArgumentNullException(nameof(userToken));

            userToken.ShiftCurrentToken();
            _userTokenRepository.Update(userToken);            
            await _userTokenRepository.SaveChangesAsync();

            return userToken;
        }

        public async Task ExpiresTokenAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var userToken = _userTokenRepository.GetByUser(user);
            if (userToken == null) return;

            userToken.ExpiresToken();
            _userTokenRepository.Update(userToken);
            await _userTokenRepository.SaveChangesAsync();
        }

        public User GetUser(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public User GetUser(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        public async Task<UserToken> GetByTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return await Task.FromResult<UserToken>(null);

            return await _userTokenRepository.GetByTokenAsync(token);
        }
    }
}
