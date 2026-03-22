using MiddleLayerAPI.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Net.Http.Formatting;

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
        /// <param name="client">HttpClient nullable</param>
        /// <returns>Null, or response from the model</returns>
        public async static Task<ModelResponse?> PostToModel(ModelInput input, HttpClient? client)
        {
            if (client is null)
            {
                client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
            }
            string jsonString = JsonSerializer.Serialize(input);
            HttpResponseMessage retval = await client.PostAsJsonAsync("http://localhost:8080/predict",  input);
            var jsonResult = retval.Content.ReadAsStringAsync().Result;
            ModelResponse? response = JsonSerializer.Deserialize<ModelResponse>(jsonString);

            return response;
        }
    }
}
