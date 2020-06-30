using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdLitica.Entities.Management;
using sdLitica.Exceptions.Http;
using sdLitica.PlatformCore;
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
        private readonly UserService _userService;
        
        /// <summary>
        /// This controller is used to work with user
        /// </summary>
        /// <param name="mysqlProfileService"></param>
        public ProfileController(UserService userService)
        {
            _userService = userService;                  
        }


        /// <summary>
        /// This API call will create a new user in the system
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<String> RegisterNewUser([FromBody] UserModel newUser) {
            
            User exitingUser = _userService.GetUser(newUser.Email);

            
            if (null == exitingUser)
            {
                _userService.CreateUser(newUser.FirstName,
                                        newUser.LastName,
                                        newUser.Email,
                                        newUser.Password);

                return "Created new user";
            }
            else
            {
                throw new UserExistsException("User with email address '" + 
                                              newUser.Email + 
                                              "' already registered in the system");
            }
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

            var user = _userService.GetUser(credentials.Email);
            if (user == null) throw new NotFoundException("User not found");

            if (!user.MatchPassword(credentials.Password))
                throw new UnauthorizedException("Incorrect password");

            var userToken = await _userService.GetNewTokenAsync(user);

            var tokenJson = new TokenJsonEntity()
            {
                Token = userToken.Token,
                Expires = userToken.TokenExpirationDate.Ticks
            };
            
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
            var user = _userService.GetUser(new Guid(UserId));
            await _userService.ExpiresTokenAsync(user);

            return NoContent();
        }

        /// <summary>
        /// This REST API handler to get details about current user 
        /// profile identified by authorization token
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        public async Task<UserModel> GetProfile()
        {
            var user = _userService.GetUser(new Guid(UserId));
            

            return new UserModel(user);
        }        
    }
}
