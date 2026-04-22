using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Controllers
{
    /// <summary>
    /// Controller for sending and recieving requests to and from the python API for predictions, as well as a health check endpoint
    /// </summary>
    [ApiController]
    [Route("Detector")]
    public class DetectorController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        public DetectorController(IOptions<AppSettings> appSettings, AppDbContext context)
        {
            _appSettings = appSettings.Value;
            _appSettings.SetDbContext(context);
        }
        /// <summary>
        /// Takes in the input from the front end
        /// Sends off to the python API and returns the response to the front end
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetPrediction([FromBody] ModelInput input)
        {
            
            ModelResponse? retval = PostToModel(input).Result;

            if(retval != null)
            {
                return new JsonResult(Ok(retval));
            }
            var errorResponse = new ModelResponse
            {
                prediction = "Error: No response from model"
            };
            return new JsonResult(NoContent());
        }
        /// <summary>
        /// Async function to post the input to the python API and get the response back, using the ApiRequestHelper class
        /// </summary>
        /// <param name="input">Statement from the front end</param>
        /// <returns>Response from the model</returns>
        private async Task<ModelResponse?> PostToModel(ModelInput input)
        {
            ModelResponse? response = await Helpers.ApiRequestHelper.PostToModel(input, _appSettings);

            return response;
        }
        /// <summary>
        /// Simple health check to make sure that the middle layer is running
        /// </summary>
        /// <returns>Ok result to show that the middle layer is running</returns>
        [HttpGet("HealthCheck")]
        public IActionResult HealthCheck()
        {
            return Ok("Middle Layer is running");
        }
    }
}
