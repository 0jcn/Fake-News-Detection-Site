using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiddleLayerAPI.Helpers;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Controllers
{
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
        [HttpGet]
        [Authorize]
        public IActionResult GetUser()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            Dictionary<string, object> decodedToken = JWTHelper.GetClaimsFromToken(token, _appSettings.JWTSecret);
            if (decodedToken.ContainsKey("userId"))
            {
                int userId = Convert.ToInt32(decodedToken["userId"]);
                var response = _appSettings.DbHelper.GetUser(userId).Result;
                if (response != null)
                {
                    return new JsonResult(Ok(response));
                }
            }
            return new JsonResult(NoContent());

        }
        private string GetToken(int id, string username, string tier)
        {
            string token = JWTHelper.GenerateToken(id, username, tier, _appSettings.JWTSecret);
            return token;
        }
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


        [HttpPut]
        [Authorize]
        public IActionResult updateUser([FromBody] UpdateUser updatedUser)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            Dictionary<string, object> decodedToken = JWTHelper.GetClaimsFromToken(token, _appSettings.JWTSecret);
            if (decodedToken.ContainsKey("userId"))
            {
                int userId = Convert.ToInt32(decodedToken["userId"]);
                var response = _appSettings.DbHelper.UpdateUser(updatedUser, userId).Result;
                if (response != null)
                {
                    return new JsonResult(Ok(response));
                }
            }
            return new JsonResult(NoContent());
        }
        [HttpDelete]
        [Authorize]
        public IActionResult deleteUser()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            Dictionary<string, object> decodedToken = JWTHelper.GetClaimsFromToken(token, _appSettings.JWTSecret);
            if (decodedToken.ContainsKey("userId"))
            {
                int userId = Convert.ToInt32(decodedToken["userId"]);
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
