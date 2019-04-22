using sdLitica.Entities.Abstractions;
using sdLitica.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace sdLitica.Entities.Management
{
    /// <summary>
    /// This class represents unique identity and info of the user
    /// </summary>
    public class User : Entity
    {
        /// <summary>
        /// User first name
        /// </summary>
        public string FirstName { get; protected set; }
        /// <summary>
        /// User last name
        /// </summary>
        public string LastName { get; protected set; }
        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; protected set; }
        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; protected set; }

        protected User()
        {

        }

        public void ChangePassword(string plainPassword)
        {
            Password = Encrypt(plainPassword);
        }

        public bool MatchPassword(string plainPassword)
        {
            return Password.Equals(Encrypt(plainPassword));
        }

        private string Encrypt(string plainPassword)
        {
            return HashHelper.GetSha256(plainPassword);
        }

        public static User Create(string firstName, string lastName, string email, string plainPassword)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email                
            };
            user.ChangePassword(plainPassword);
            return user;
        }
    }
}
