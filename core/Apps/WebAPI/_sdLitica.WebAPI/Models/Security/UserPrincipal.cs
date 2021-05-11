using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace sdLitica.WebAPI.Security
{
    /// <summary>
    /// Basic user principal to represent user in
    /// security schemas
    /// </summary>
    public class UserPrincipal : IPrincipal
    {
        /// <summary>
        /// Identity object to identify user
        /// for the given rincipal
        /// </summary>
        public IIdentity Identity { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="profileId"></param>
        public UserPrincipal(String profileId)
        {
            Identity = new ClaimsIdentity(
                new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, profileId)
                });
        }

        

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
