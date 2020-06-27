﻿using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace sdLitica.WebAPI.Controllers.v1
{
    /// <summary>
    /// controller for all sub controllers
    /// </summary>
    public class BaseApiController: Controller
    {
        /// <summary>
        /// current user id
        /// </summary>
        //public string UserId => HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public string UserId => ((Claim)((ClaimsIdentity)User.Identity).Claims.First()).Value.ToString();        
    }
}
