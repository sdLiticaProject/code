using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdLitica.CommonApiServices.ApiVersion.JsonAPI.Pages;
using sdLitica.Entities.Management;
using sdLitica.Exceptions.Http;
using sdLitica.PlatformCore;
using sdLitica.WebAPI.Models.Management;

namespace sdLitica.WebAPI.Controllers.v1
{
    /// <summary>
    /// This controller is used to work with user authentication and authorization
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/Profile")]
    [Authorize]
    public class ProfileController: BaseApiController
    {
        private readonly UserService _userService;

        /// <summary>
        /// Main C-tor for the controller instance
        /// </summary>
        /// <param name="userService">Auto-injected service to interact with database</param>
        public ProfileController(UserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// This REST API call will create a new user in the system
        /// </summary>
        /// <remarks>
        /// When this API call is used, it will create a new user that will be able
        /// to login to the system. Email address used for registration should be unique,
        /// since it is used as a login name.
        /// </remarks>
        /// <param name="newUser">An object describing new user to be created in the system</param>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If there were issues with request payload</response>
        /// <response code="409">If specified email address already registered in the system</response> 
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<UserModel> RegisterNewUser([FromBody] UserModel newUser)
        {
            User userInTheSystem = _userService.GetUser(newUser.Email);

            if (userInTheSystem == null)
            {
                userInTheSystem = _userService.CreateUser(newUser.FirstName,
                    newUser.LastName,
                    newUser.Email,
                    newUser.Password);

                this.HttpContext.Response.StatusCode = StatusCodes.Status201Created;
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
        /// This REST API call updates a first and last names of the current user
        /// </summary>
        /// <remarks>
        /// This endpoint allows to update user data in system.
        /// </remarks>
        /// <param name="updatedUser">An object describing new user info to be updated in the system</param>
        /// <response code="200">When user info was successfully updated</response>
        /// <response code="400">When request payload is malformed or required fields are missing</response>
        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<UserModel> UpdateUser([FromBody] UserUpdateModel updatedUser)
        {
            User user = _userService.UpdateUser(new Guid(UserId), updatedUser.FirstName, updatedUser.LastName);
            return new UserModel(user);
        }

        /// <summary>
        /// This REST API handler logins the user in the system
        /// </summary>
        /// <remarks>
        /// This endpoint allows retrieval of access token for valid pair of
        /// email and password for registered user. If provided credentials are
        /// valid, server will return an object containing token value and its
        /// expiration time. Each time when token successfully used, its expiration
        /// time will extended.
        /// </remarks>
        /// <param name="credentials">End-user credentials to identify user for whom token will be generated</param>
        /// <response code="200">When user was successfully logged in and token was issues</response>
        /// <response code="400">When request payload is malformed or required fields are missing</response>
        /// <response code="401">When request is valid, but credentials were not recognized by server</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ApiEntityPage<TokenJsonEntity>> Login([FromBody] LoginModel credentials)
        {
            // Here should be proper auth via DB with creation of token if auth succeeds

            User user = _userService.GetUser(credentials.Email);
            if (user == null) throw new UnauthorizedException("Provided credentials are not valid");

            if (!user.MatchPassword(credentials.Password))
                throw new UnauthorizedException("Provided credentials are not valid");

            UserToken userToken = await _userService.GetNewTokenAsync(user);

            TokenJsonEntity tokenJson = new TokenJsonEntity()
            {
                Token = userToken.Token,
                Expires = userToken.TokenExpirationDate.Ticks
            };

            ApiEntityPage<TokenJsonEntity> result = new ApiEntityPage<TokenJsonEntity>(tokenJson,
                HttpContext.Request.Path.ToString());

            return result;
        }

        /// <summary>
        /// This REST API call logouts the user out of the system for the given token
        /// </summary>
        /// <remarks>
        /// When this request is authenticated with valid token it 
        /// will log user out by immediate decommissioning of provided
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
            User user = _userService.GetUser(new Guid(UserId));
            await _userService.ExpiresTokenAsync(user);

            return NoContent();
        }

        /// <summary>
        /// This REST API call returns the current user profile
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
            User user = _userService.GetUser(new Guid(UserId));
            return new UserModel(user);
        }

        /// <summary>
        /// List available API keys
        /// </summary>
        /// <remarks>
        /// This REST API handler returns list of API keys  
        /// created by the authenticated users. These keys can 
        /// be used by other applications to access platfrom API's
        /// </remarks>
        /// <response code="200">When valid token is provided, list of available api keys</response>
        /// <response code="401">When authentication token is missing or invalid</response>
        [HttpGet("apikeys")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ApiEntityListPage<UserApiKeyJsonEntity> GetApiKeys()
        {
            List<UserApiKey> apiKeys = _userService.GetUserApiKeys(UserId);

            // Converting to unified fancy JSON
            List<UserApiKeyJsonEntity> apiKeyJsonEntities = 
                    apiKeys.ConvertAll<UserApiKeyJsonEntity>(key => new UserApiKeyJsonEntity(key));

            ApiEntityListPage<UserApiKeyJsonEntity> result = 
                new ApiEntityListPage<UserApiKeyJsonEntity>(apiKeyJsonEntities, Request.Path.ToString());

            return result;
        }


        /// <summary>
        /// Create new API key 
        /// </summary>
        /// <remarks>
        /// This REST API handler returns current user 
        /// profile for user identified by authorization token
        /// </remarks>
        /// <response code="201">When valid token is provided, profile for the user identified by the token</response>
        /// <response code="401">When authentication token is missing or invalid</response>
        [HttpPost("apikeys")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ApiEntityPage<UserApiKeyJsonEntity> CreateApiKey([FromBody] IDictionary<string, string> requestBody)
        {
            // In general it is not a good idea to parse JSON object into the dictionary,
            // but here we need to have only on field with description, so there is
            // no need to create an object. Maybe in the fitire when we decide to have
            // mode fields here - we will convert this to a normal input object.
            string keyDescription = requestBody.ContainsKey("description") ? 
                                        requestBody["description"] : 
                                        "not set";

            UserApiKey apiKey = _userService.CreateUserApiKey(UserId, keyDescription);

            // Convreting to fancy JSON
            ApiEntityPage<UserApiKeyJsonEntity> result = 
                    new ApiEntityPage<UserApiKeyJsonEntity>(new UserApiKeyJsonEntity(apiKey), Request.Path.ToString());

            Response.StatusCode = StatusCodes.Status201Created;

            return result;
        }

        /// <summary>
        /// Delete an API key by its Id
        /// </summary>
        /// <remarks>
        /// This REST API handler allows user to delete an existing 
        /// API key by its Id
        /// </remarks>
        /// <response code="204">When API key successfully deleted</response>
        /// <response code="401">When authentication token is missing or invalid</response>
        /// <response code="404">When API key was not found at specified Id</response>
        [HttpDelete("apikeys/{keyId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public void DeleteApiKey([FromRoute] string keyId) {
            
            bool result = _userService.DeleteUserApiKey(keyId);

            if (!result)
            {
                throw new NotFoundException();
            }

            Response.StatusCode = StatusCodes.Status204NoContent;
        }
    }
}