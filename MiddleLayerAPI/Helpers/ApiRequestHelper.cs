using MiddleLayerAPI.Models;
using System.Text.Json;

namespace MiddleLayerAPI.Helpers
{
    /// <summary>
    /// Static class for creating and sending requests to the python API for predictions    
    /// </summary>
    public static class ApiRequestHelper
    {
        /// <summary>
        /// Posts a request to the python API for predictions
        /// </summary>
        /// <param name="input">Input from the frontend</param>
        /// <returns>Null, or response from the model</returns>
        public async static Task<ModelResponse?> PostToModel(ModelInput input, AppSettings appSettings)
        {
            var client = appSettings.Client;   
            string apiUrl = appSettings.ApiUrl;
            string endpoint = $"{apiUrl}predict";
            string jsonString = JsonSerializer.Serialize(input);
            HttpResponseMessage retval = await client.PostAsJsonAsync(endpoint,  input);
            var jsonResult = retval.Content.ReadAsStringAsync().Result;
            ModelResponse? response = JsonSerializer.Deserialize<ModelResponse>(jsonResult);

            return response;
        }
    }
}
