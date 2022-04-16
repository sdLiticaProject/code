using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sdLitica.CommonApiServices.ApiVersion.JsonAPI;

namespace sdLitica.WebAPI.Controllers.v1
{
    /// <summary>
    /// This controller is a sample conmtroller that requires authentication
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/sample")]
    [Authorize]
    public class SampleController : BaseApiController
    {
        

        /// <summary>
        /// This REST API handler returns the list of domain projects
        /// </summary>
        /// <returns>List of domain projects</returns>
        [HttpGet]
        public async Task<NoContentResult> Get([FromQuery] PaginationProperties pagination)
        {
            return NoContent();
        }
        
    }
}