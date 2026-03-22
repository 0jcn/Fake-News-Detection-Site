using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        /// <summary>
        /// Takes in the input from the front end
        /// Sends off to the python API and returns the response to the front end
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "Predict")]
        public ModelResponse GetPrediction([FromBody] ModelInput input)
        {
            
            ModelResponse? retval = PostToModel(input).Result;

            if(retval != null)
            {
                return retval;
            }
            return new ModelResponse
            {
                prediction = "Error: No response from model"
            };
        }

        private async Task<ModelResponse?> PostToModel(ModelInput input)
        {
            ModelResponse? response = await Helpers.ApiRequestHelper.PostToModel(input, null);

            return response;
        }

        [HttpGet(Name = "HealthCheck")]
        public IActionResult HealthCheck()
        {
            return Ok("Middle Layer is running");
        }
    }
}
