using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace sdLitica.CommonApiServices.Security
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
        /// <param name="userId"></param>
        public UserPrincipal(Guid userId)
        {
            Identity = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                });
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}