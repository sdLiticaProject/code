using sdLitica.Entities.Abstractions;
using System;
using System.Collections.Generic;
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
        public string FirstName { get; set; }
        /// <summary>
        /// User last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }
    }
}
