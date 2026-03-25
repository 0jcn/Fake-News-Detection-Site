namespace MiddleLayerAPI.Models
{
    /// <summary>
    /// Model for storing the appsettings needed across the API
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// URL to the machine learning API for predictions
        /// </summary>
        public string ApiUrl { get; set; } = string.Empty;
        /// <summary>
        /// Http Client to use for requests to the machine learning API for predictions
        /// </summary>
        public HttpClient Client => GetClient();
        /// <summary>
        /// Returns a new HttpClient with the appropriate headers for requests to the machine learning API for predictions
        /// </summary>
        /// <returns>HttpClient for predictions</returns>
        public HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

    }
}
