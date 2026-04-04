using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Controllers
{
    [ApiController]
    [Route("Detections")]
    public class SaveDetectionsController : Controller
    {
        private readonly AppSettings _appSettings;
        public SaveDetectionsController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        [HttpPost]
        public IActionResult SaveDetection([FromBody] SavedDetections detection)
        {
            var response = _appSettings.DbHelper.SaveDetection(detection).Result;
            if (response)
            {
                return new JsonResult(Ok("Detection saved successfully"));
            }
            return new JsonResult(NoContent());
        }
        [HttpDelete("/{detectionId}")]
        public IActionResult RemoveSavedDetection(int detectionId)
        {
            var response = _appSettings.DbHelper.RemoveSavedDetection(detectionId).Result;
            if (response)
            {
                return new JsonResult(Ok("Detection removed successfully"));
            }
            return new JsonResult(NoContent());
        }
    }
}
