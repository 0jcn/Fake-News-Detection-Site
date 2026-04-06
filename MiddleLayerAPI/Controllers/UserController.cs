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
                        return new JsonResult(Ok(response));
                    }
                }
                }
                return new JsonResult(NoContent());

        }
        [HttpGet("/{userId}")]
        public IActionResult GetUser(int userId)
        {
            var response = _appSettings.DbHelper.GetUser(userId).Result;
            if (response != null)
            {
                return new JsonResult(Ok(response));
            }
            return new JsonResult(NoContent());

        }

        [HttpPost("/login")]
        public IActionResult Login([FromBody] Login loginRequest)
        {
            var user = _appSettings.DbHelper.GetUserByUsername(loginRequest.Username).Result;
            if (user != null && PasswordHelper.VerifyPassword(loginRequest.Password, user.Password))
            {
                return new JsonResult(Ok(user));
            }
            return new JsonResult(NoContent());
        }

        [HttpPut]
        public IActionResult updateUser([FromBody] Users updatedUser)
        {
            var response = _appSettings.DbHelper.UpdateUser(updatedUser).Result;
            if (response != null)
            {
                return new JsonResult(Ok(response));
            }
            return new JsonResult(NoContent());
        }
        [HttpDelete("/{userId}")]
        public IActionResult deleteUser(int userId)
        {
            var response = _appSettings.DbHelper.DeleteUser(userId).Result;
            if (response)
            {
                return new JsonResult(Ok("User deleted successfully"));
            }
            return new JsonResult(NoContent());
        }
    }
}
