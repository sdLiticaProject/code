
using sdLitica.Entities.Abstractions;
using sdLitica.Utils.Helpers;
using System;

namespace sdLitica.Entities.Management
{
    /// <summary>
    /// This class is used to describe UserToken entity
    /// </summary>
    public class UserToken : Entity
    {
        /// <summary>
        /// User token
        /// </summary>
        public string Token { get; protected set; }
        /// <summary>
        /// User token expiration date
        /// </summary>
        public DateTime TokenExpirationDate { get; protected set; }
        /// <summary>
        /// User Id identifier
        /// </summary>
        public Guid UserId { get; protected set; }
        /// <summary>
        /// User Id identifier
        /// </summary>
        public User User { get; protected set; }

        /// <summary>
        /// Creates an UserToken with no parameters
        /// </summary>
        protected UserToken()
        {

        }

        /// <summary>
        /// This method expires the current token in the UserToken object
        /// </summary>
        public void ExpiresToken()
        {
            var now = DateTime.UtcNow;            
            TokenExpirationDate = now.AddDays(-1);
        }

        /// <summary>
        /// This method check if the current token is expired, using current UTC datetime
        /// </summary>
        /// <returns>True if the token is expired</returns>
        public bool IsTokenExpired()
        {
            return DateTime.UtcNow > TokenExpirationDate;
        }

        /// <summary>
        /// This method creates a Token
        /// </summary>
        /// <param name="expiration">Expiration in hours</param>
        public void CreateToken(int expiration = 3)
        {
            var now = DateTime.UtcNow;
            Token = HashHelper.GetSha256(Convert.ToString(now.Ticks));
            TokenExpirationDate = now.AddHours(expiration);
        }

        /// <summary>
        /// This method shifts current token expiration
        /// </summary>
        /// <param name="expiration">Expiration in hours</param>
        public void ShiftCurrentToken(int expiration = 3)
        {
            var now = DateTime.UtcNow;            
            TokenExpirationDate = now.AddHours(expiration);
        }

        /// <summary>
        /// This factory method creates an UserToken to the parameterized user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserToken Create(User user)
        {
            var userToken = new UserToken()
            {
                Id = Guid.NewGuid(),
                User = user ?? throw new ArgumentNullException(nameof(user)),
                UserId = user.Id,                
            };

            userToken.CreateToken();
            return userToken;
        }
    }
}
