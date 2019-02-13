using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdLitica.Entities.Management;
using sdLitica.WebAPI.Entities.Common.Pages;
using sdLitica.WebAPI.Models.Management;

namespace sdLitica.WebAPI.Controllers.v1
{
    /// <summary>
    /// This controller is used to work with user authentification and authorization
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/Profile")]
    [Authorize]
    public class ProfileController : BaseApiController
    {
        /*private readonly IMySQLPersistaenceService _mysqlProfileService;*/
        /// <summary>
        /// This controller is used to work with user
        /// </summary>
        /// <param name="mysqlProfileService"></param>
        public ProfileController(/*IMySQLPersistaenceService mysqlProfileService*/)
        {
            //_mysqlProfileService = mysqlProfileService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentials"></param>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ApiEntityPage<TokenJsonEntity>> Login([FromBody] LoginModel credentials)
        {
            // Here should be proper auth via DB with clreation of token if auth succeeds
            

            
            ProfileToken profileToken = new ProfileToken();
            TokenJsonEntity tokenJson = new TokenJsonEntity()
            {
                Token = "fake-token-value",
                Expires = 0
            };

            if (credentials.Name != "testUser" || credentials.Password != "password")
            {
                throw new Exceptions.UnauthorizedException("User is not authorized");
            }

            ApiEntityPage<TokenJsonEntity> result =
                                new ApiEntityPage<TokenJsonEntity>(tokenJson,
                                                                    HttpContext.Request.Path.ToString());

            return result;
        }

        /// <summary>
        /// This REST API handler sign out user from system
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<NoContentResult> Logout()
        {
            return NoContent();
        }
        
    }
}
