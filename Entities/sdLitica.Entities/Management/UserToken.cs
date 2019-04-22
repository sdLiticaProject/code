
using sdLitica.Entities.Abstractions;
using sdLitica.Utils.Helpers;
using System;

namespace sdLitica.Entities.Management
{
    /// <summary>
    /// This class used to describe UserToken entity in database
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

        protected UserToken()
        {

        }

        public void ExpiresToken()
        {
            var now = DateTime.UtcNow;            
            TokenExpirationDate = now.AddDays(-1);
        }

        public bool IsTokenExpired()
        {
            return DateTime.UtcNow > TokenExpirationDate;
        }

        public void CreateToken(int expiration = 3)
        {
            var now = DateTime.UtcNow;
            Token = HashHelper.GetSha256(Convert.ToString(now.Ticks));
            TokenExpirationDate = now.AddHours(expiration);
        }

        public void ShiftCurrentToken(int expiration = 3)
        {
            var now = DateTime.UtcNow;            
            TokenExpirationDate = now.AddHours(expiration);
        }

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
