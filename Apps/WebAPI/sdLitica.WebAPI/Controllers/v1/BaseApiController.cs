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
        public string UserId => User.Identity.Name;        
    }
}
