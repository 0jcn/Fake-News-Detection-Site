using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController : Controller
    {
        private readonly AppSettings _appSettings;

        public UserController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        [HttpPost]
        public IActionResult CreateUser([FromBody] Users newUser)
        {
            var response = _appSettings.DbHelper.CreateUser(newUser).Result;
            if (response != null)
            {
                return new JsonResult(Ok(response));
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
