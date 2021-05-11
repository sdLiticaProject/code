using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sdLitica.Entities.Management;
using sdLitica.Entities.Management.Repositories;
using sdLitica.Exceptions.Http;
using sdLitica.Exceptions.Managements;
using sdLitica.Utils.Abstractions;

namespace sdLitica.PlatformCore
{
    /// <summary>
    /// This class provides services for User and UserToken management. These services can access the database.
    /// </summary>
    public class UserService
    {
        private readonly IAppSettings _appSettings;
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IUserApiKeyRepository _userApiKeyRepository;

        /// <summary>
        /// Creates an UserService. 
        /// Please, do not instantiate this class, instead get it from dependency injection container
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="userRepository"></param>
        /// <param name="userTokenRepository"></param>
        /// <param name="userApiKeyRepository"></param>
        public UserService(IAppSettings appSettings, 
                            IUserRepository userRepository,
                            IUserTokenRepository userTokenRepository,
                            IUserApiKeyRepository userApiKeyRepository)
        {
            _appSettings = appSettings;
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _userApiKeyRepository = userApiKeyRepository;
        }

        /// <summary>
        /// Creates a simple user
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="plainPassword"></param>
        public User CreateUser(string firstName, string lastName, string email, string plainPassword)
        {
            if (_userRepository.Exists(email))
                throw new PropertyDuplicationException("Email", email);
            
            User user = User.Create(firstName, lastName, email, plainPassword);            
            _userRepository.Add(user);
            _userRepository.SaveChanges();
            return user;
        }

        /// <summary>
        /// Updates a first and last names of user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public User UpdateUser(Guid id, string firstName, string lastName)
        {
            User user = GetUser(id);
            if (user == null) throw new NotFoundException("User not found");
            user.Update(firstName, lastName);
            _userRepository.Update(user);
            _userRepository.SaveChanges();
            return user;
        }

        /// <summary>
        /// Get a new token for the parametrized user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UserToken> GetNewTokenAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            UserToken userToken = _userTokenRepository.GetByUser(user);
            if (userToken == null)
            {
                userToken = UserToken.Create(user, _appSettings.TokenExpirationInHours);
                _userTokenRepository.Add(userToken);
            }
            else
            {
                userToken.CreateToken(_appSettings.TokenExpirationInHours);
                _userTokenRepository.Update(userToken);
            }
            await _userTokenRepository.SaveChangesAsync();
            return userToken;
        }

        /// <summary>
        /// This method shifts the current token in UserToken as parameterized in `appsettings`
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public async Task<UserToken> ShiftTokenAsync(UserToken userToken)
        {
            if (userToken == null) throw new ArgumentNullException(nameof(userToken));

            userToken.ShiftCurrentToken(_appSettings.TokenExpirationInHours);
            _userTokenRepository.Update(userToken);            
            await _userTokenRepository.SaveChangesAsync();

            return userToken;
        }

        /// <summary>
        /// This method expires the token for the given user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task ExpiresTokenAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            UserToken userToken = _userTokenRepository.GetByUser(user);
            if (userToken == null) return;

            userToken.ExpiresToken();
            _userTokenRepository.Update(userToken);
            await _userTokenRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUser(Guid id)
        {
            return _userRepository.GetById(id);
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetUser(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        /// <summary>
        /// This method get an UserToken entity for given token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserToken> GetByTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return await Task.FromResult<UserToken>(null);

            return await _userTokenRepository.GetByTokenAsync(token);
        }

        /// <summary>
        /// This method get an UserApiKey entity for given api key value
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserApiKey> GetByApiKeyAsync(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey)) return await Task.FromResult<UserApiKey>(null);

            return await _userApiKeyRepository.GetByApiKeyAsync(apiKey);
        }

        /// <summary>
        /// Get the list of ApiKey entities owned by en-user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<UserApiKey> GetUserApiKeys(string userId)
        {
            return _userApiKeyRepository.GetByUserId(new Guid(userId));
        }


        /// <summary>
        /// Get the list of ApiKey entities owned by en-user
        /// </summary>
        /// <param name="userId">Id of the user for whome new key will be issued</param>
        /// <param name="description">Descritpion of an API key to be created</param>
        /// <returns></returns>
        public UserApiKey CreateUserApiKey(string userId, string description)
        {
            UserApiKey newKey = UserApiKey.CreateNew(_userRepository.GetById(new Guid(userId)), description);
            _userApiKeyRepository.Add(newKey);
            _userRepository.SaveChanges();
            return _userApiKeyRepository.GetById(newKey.Id);
        }

        /// <summary>
        /// Delete an API key by its Id
        /// </summary>
        /// <param name="apiKeyId">Id of the API key to be deleted</param>
        /// <returns>True if key was deleted. Otherwise false</returns>
        public bool DeleteUserApiKey(string apiKeyId)
        {
            bool result = false;

            UserApiKey apiKey = _userApiKeyRepository.GetById(new Guid(apiKeyId));
            if (null != apiKey)
            {
                _userApiKeyRepository.Delete(apiKey);
                _userApiKeyRepository.SaveChanges();
                result = true;
            }

            return result;
        }
    }
}
