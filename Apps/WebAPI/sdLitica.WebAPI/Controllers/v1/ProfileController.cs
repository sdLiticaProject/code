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
        /// Main C-tor for the controller instance
        /// </summary>
        /// <param name="mysqlProfileService">Auto-injected service to interact with database</param>
        public ProfileController(UserService userService)
        {
            _userService = userService;                  
        }


        /// <summary>
        /// This API call will create a new user in the system
        /// </summary>
        /// <remarks>
        /// When this API call is used, it will create a new user that will be able
        /// to login to the system. Email address used for registraton should be unique,
        /// since it is used as a login name.
        /// </remarks>
        /// <param name="newUser">An object describing new user to be created in the system</param>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If there were issues with request payload</response>
        /// <response code="409">If specified email address already registered in the system</response> 
        [HttpPost]
        [AllowAnonymous]
        public async Task<UserModel> RegisterNewUser([FromBody] UserModel newUser) {
            
            User userInTheSystem = _userService.GetUser(newUser.Email);

            
            if (null == userInTheSystem)
            {
                userInTheSystem = _userService.CreateUser(newUser.FirstName,
                                        newUser.LastName,
                                        newUser.Email,
                                        newUser.Password);

                this.HttpContext.Response.StatusCode = 201;
                return new UserModel(userInTheSystem);
            }
            else
            {
                throw new UserExistsException("User with email address '" + 
                                              newUser.Email + 
                                              "' already registered in the system");
            }
        }

        /// <summary>
        /// Log in user to the system
        /// </summary>
        /// <remarks>
        /// This endpoint allows retrieval of access token for valid pair of
        /// email and password for registered user. If provided credentials are
        /// vlid, server will return an object containing token value and its
        /// expiration time. Each time when token successfully used, its expiration
        /// time will extended.
        /// </remarks>
        /// <param name="credentials">End-user credentials to identify user for whome token will be generated</param>
        /// <response code="200">When user was successfully logged in and token was issues</response>
        /// <response code="400">When request payload is malformed or reuired fields are missing</response>
        /// <response code="401">When request is valid, but credentials were not recognized by server</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// Log out from the system for the given token
        /// </summary>
        /// <remarks>
        /// When this request is authenticated with valid token it 
        /// will log user out by imediate decomissioning of provided
        /// token. After using of this endpoint, provided token
        /// is no more valid
        /// </remarks>
        /// <response code="204">When user was successfully logged out</response>
        /// <response code="401">When authentication token is missing or invalid</response>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<NoContentResult> Logout()
        {
            var user = _userService.GetUser(new Guid(UserId));
            await _userService.ExpiresTokenAsync(user);

            return NoContent();
        }

        /// <summary>
        /// Get current user profile
        /// </summary>
        /// <remarks>
        /// This REST API handler returns current user 
        /// profile for user identified by authorization token
        /// </remarks>
        /// <response code="200">When valid token is provided, profile for the user identified by the token</response>
        /// <response code="401">When authentication token is missing or invalid</response>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<UserModel> GetProfile()
        {
            var user = _userService.GetUser(new Guid(UserId));
            

            return new UserModel(user);
        }        
    }
}
