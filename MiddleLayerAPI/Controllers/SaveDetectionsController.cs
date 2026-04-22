using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiddleLayerAPI.Helpers;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Controllers
{
    /// <summary>
    /// Controller for saving detections
    /// Only accessible when the user has loggged in
    /// </summary>
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
        /// <summary>
        /// Saves a detection for the logged in user
        /// </summary>
        /// <param name="detection">Detection to save</param>
        /// <returns>Ok if the detection is saved successfully, BadRequest otherwise</returns>
        [HttpPost]
        public IActionResult SaveDetection([FromBody] SavedDetections detection)
        {
            var userId = JWTHelper.GetUserIdFromToken(Request, _appSettings.JWTSecret);
            if (userId != -1)
            {
                detection.UserId = userId;
                var response = _appSettings.DbHelper.SaveDetection(detection).Result;
                if (response)
                {
                    return new JsonResult(Ok("Detection saved successfully"));
                }
            }
            return new JsonResult(BadRequest());
        }
        /// <summary>
        /// Deletes a saved detection for the user
        /// </summary>
        /// <param name="detectionId">Detection ID</param>
        /// <returns>Ok or NoContent</returns>
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
        /// <summary>
        /// Retrieves all of a users saved detections
        /// </summary>
        /// <returns>List of saved detections if successful, NoContent otherwise</returns>
        [HttpGet]
        public IActionResult GetSavedDetections()
        {
            var userId = JWTHelper.GetUserIdFromToken(Request, _appSettings.JWTSecret);
            if (userId != -1)
            {
                var response = _appSettings.DbHelper.GetSavedDetectionsByUserId(userId).Result;
                if (response != null)
                {
                    return new JsonResult(Ok(response));
                }
            }
            return new JsonResult(NoContent());
        }
    }
}
