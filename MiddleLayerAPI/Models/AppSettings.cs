using MiddleLayerAPI.Interfaces;
using MiddleLayerAPI.Helpers;

namespace MiddleLayerAPI.Models
{
    /// <summary>
    /// Model for storing the appsettings needed across the API
    /// </summary>
    public class AppSettings
    {
        private HttpClient _httpClient;
        private IDatabaseHelper _databaseHelper;
        public AppSettings()
        {
            _httpClient = GetClient();
        }

        public void SetDbContext(AppDbContext context)
        {
            _databaseHelper = new DatabaseHelper(context);
        }
        /// <summary>
        /// URL to the machine learning API for predictions
        /// </summary>
        public string ApiUrl { get; set; } = string.Empty;
        /// <summary>
        /// Http Client to use for requests to the machine learning API for predictions
        /// </summary>
        public HttpClient Client => _httpClient;
        public IDatabaseHelper DbHelper => _databaseHelper;

        public string DbConnectionString { get; set; }

        public string JWTSecret { get; set; }
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
