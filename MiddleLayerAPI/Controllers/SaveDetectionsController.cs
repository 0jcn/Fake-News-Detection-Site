using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiddleLayerAPI.Helpers;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("SaveDetections")]
    public class SaveDetectionsController : Controller
    {
        private readonly AppSettings _appSettings;
        public SaveDetectionsController(IOptions<AppSettings> appSettings, AppDbContext context)
        {
            _appSettings = appSettings.Value;
            _appSettings.SetDbContext(context);
        }
        [HttpPost]
        public IActionResult SaveDetection([FromBody] SavedDetections detection)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            Dictionary<string, object> decodedToken = JWTHelper.GetClaimsFromToken(token, _appSettings.JWTSecret);
            if (decodedToken.ContainsKey("userId"))
            {
                var userId = decodedToken["userId"];
                detection.UserId = Convert.ToInt32(userId);

                var response = _appSettings.DbHelper.SaveDetection(detection).Result;
                if (response)
                {
                    return new JsonResult(Ok("Detection saved successfully"));
                }
            }
            return new JsonResult(BadRequest());
        }
        [HttpDelete("{detectionId}")]
        public IActionResult RemoveSavedDetection(int detectionId)
        {
            var response = _appSettings.DbHelper.RemoveSavedDetection(detectionId).Result;
            if (response)
            {
                return new JsonResult(Ok("Detection removed successfully"));
            }
            return new JsonResult(NoContent());
        }

        [HttpGet]
        public IActionResult GetSavedDetections()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            Dictionary<string, object> decodedToken = JWTHelper.GetClaimsFromToken(token, _appSettings.JWTSecret);
            if (decodedToken.ContainsKey("userId"))
            {
                var userId = decodedToken["userId"];
                var response = _appSettings.DbHelper.GetSavedDetectionsByUserId(Convert.ToInt32(userId)).Result;
                if (response != null)
                {
                    return new JsonResult(Ok(response));
                }
            }
            return new JsonResult(NoContent());
        }
    }
}
