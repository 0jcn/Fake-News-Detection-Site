using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiddleLayerAPI.Models;
using Newtonsoft.Json;

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
        public DetectorController(IOptions<AppSettings> appSettings) 
        {
            _appSettings = appSettings.Value;
        }
        /// <summary>
        /// Takes in the input from the front end
        /// Sends off to the python API and returns the response to the front end
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "Predict")]
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

        private async Task<ModelResponse?> PostToModel(ModelInput input)
        {
            ModelResponse? response = await Helpers.ApiRequestHelper.PostToModel(input, _appSettings);

            return response;
        }

        [HttpGet(Name = "HealthCheck")]
        public IActionResult HealthCheck()
        {
            return Ok("Middle Layer is running");
        }
    }
}
