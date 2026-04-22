using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiddleLayerAPI.Helpers;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Controllers
{
    /// <summary>
    /// Controller for handling user operations
    /// </summary>
    [ApiController]
    [Route("Users")]
    public class UserController : Controller
    {
        private readonly AppSettings _appSettings;

        public UserController(IOptions<AppSettings> appSettings, AppDbContext context)
        {
            _appSettings = appSettings.Value;
            _appSettings.SetDbContext(context);
        }
        /// <summary>
        /// Creates a new user account
        /// </summary>
        /// <param name="newUser">New user details</param>
        /// <returns>JWT token if the user is created successfully, BadRequest otherwise</returns>
        [HttpPost]
        public IActionResult CreateUser([FromBody] Users newUser)
        {
            if (newUser != null)
            {
                string? hashedPassword = PasswordHelper.HashPassword(newUser.Password);
                if (hashedPassword != null) {
                    newUser.Password = hashedPassword;
                    var response = _appSettings.DbHelper.CreateUser(newUser).Result;
                    if (response != null)
                    {
                        string token = GetToken(response.Id, response.Username, response.Tier);
                        return new JsonResult(Ok(new Dictionary<string, string>() { { "token", token } }));
                    }
                }
                }
                return BadRequest();

        }
        /// <summary>
        /// Retrieves the details of the currently authenticated user
        /// </summary>
        /// <returns>User details if the token is valid, NoContent otherwise</returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetUser()
        {
            int userId = JWTHelper.GetUserIdFromToken(Request, _appSettings.JWTSecret);
            if (userId != -1)
            {
                var response = _appSettings.DbHelper.GetUser(userId).Result;
                if (response != null)
                {
                    return new JsonResult(Ok(response));
                }
            }
            return new JsonResult(NoContent());

        }
        /// <summary>
        /// Generates a JWT for the specified user and access tier.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="username">Users username</param>
        /// <param name="tier">Users tier</param>
        /// <returns>A string containing the generated JWT for the specified user and tier.</returns>
        private string GetToken(int id, string username, string tier)
        {
            string token = JWTHelper.GenerateToken(id, username, tier, _appSettings.JWTSecret);
            return token;
        }
        /// <summary>
        /// Finds user by their username and verifies password
        /// If details are correct, generates and returns a JWT for the user
        /// </summary>
        /// <param name="loginRequest">Username and entered password</param>
        /// <returns>JWT token if successful, nothing otherwise</returns>
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] Login loginRequest)
        {
            var user = await _appSettings.DbHelper.GetUserByUsername(loginRequest.Username);
            if (user != null && PasswordHelper.VerifyPassword(loginRequest.Password, user.Password))
            {
                string token = GetToken(user.Id, user.Username, user.Tier);
                return new JsonResult(Ok(new Dictionary<string, string>() { {"token", token }  }));
            }
            return new JsonResult(NoContent());
        }

        /// <summary>
        /// Updates user details
        /// </summary>
        /// <param name="updatedUser">Updated details</param>
        /// <returns>Updated user details if successful, NoContent otherwise</returns>
        [HttpPut]
        [Authorize]
        public IActionResult updateUser([FromBody] UpdateUser updatedUser)
        {
            int userId = JWTHelper.GetUserIdFromToken(Request, _appSettings.JWTSecret);
            if (userId != -1)
            {
                var response = _appSettings.DbHelper.UpdateUser(updatedUser, userId).Result;
                if (response != null)
                {
                    return new JsonResult(Ok(response));
                }
            }
            return new JsonResult(NoContent());
        }
        /// <summary>
        /// Deletes users account
        /// </summary>
        /// <returns>Ok, or no content</returns>
        [HttpDelete]
        [Authorize]
        public IActionResult deleteUser()
        {
            int userId = JWTHelper.GetUserIdFromToken(Request, _appSettings.JWTSecret);
            if (userId != -1)
            {
                var response = _appSettings.DbHelper.DeleteUser(userId).Result;
                if (response)
                {
                    return new JsonResult(Ok("User deleted successfully"));
                }
            }
            return new JsonResult(NoContent());
        }
    }
}
