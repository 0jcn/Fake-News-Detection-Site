using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MiddleLayerAPI.Models;
using Xunit;

namespace MiddleLayerAPI.Tests.Controllers
{
    /// <summary>
    /// Integration tests for the DetectorController
    /// Tests the whole controller and ensures that connection to the python API is working, as well as the health check endpoint
    /// </summary>
    public class DetectorControllerIntegrationTests : IAsyncLifetime
    {
        private WebApplicationFactory<Program>? _factory;
        private HttpClient? _client;

        public async Task InitializeAsync()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            _client?.Dispose();
            _factory?.Dispose();
            await Task.CompletedTask;
        }

        [Fact]
        public async Task GetPrediction_WithValidInput_ReturnsOkResult()
        {
            var input = new ModelInput { statement = "This is a test news statement" };

         
            var response = await _client!.PostAsJsonAsync("/Detector", input);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NoContent,
                $"Expected success or NoContent status, got {response.StatusCode}");
        }

        [Fact]
        public async Task GetPrediction_WithNullStatement_ReturnsResponse()
        {
            var input = new ModelInput { statement = null };

            var response = await _client!.PostAsJsonAsync("/Detector", input);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetPrediction_WithEmptyStatement_ReturnsResponse()
        {
            var input = new ModelInput { statement = "" };

            var response = await _client!.PostAsJsonAsync("/Detector", input);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetPrediction_WithLongStatement_ReturnsResponse()
        {
            var longStatement = string.Concat(Enumerable.Repeat("Test statement. ", 100));
            var input = new ModelInput { statement = longStatement };

             var response = await _client!.PostAsJsonAsync("/Detector", input);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetPrediction_ReturnsJsonContentType()
        {
            var input = new ModelInput { statement = "Test statement" };

            var response = await _client!.PostAsJsonAsync("/Detector", input);

            Assert.NotNull(response);
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Contains("application/json", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task HealthCheck_ReturnsOkResult()
        {
            var response = await _client!.GetAsync("/Detector/HealthCheck");

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HealthCheck_ReturnsCorrectMessage()
        {
            var response = await _client!.GetAsync("/Detector/HealthCheck");
            var content = await response.Content.ReadAsStringAsync();

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Middle Layer is running", content);
        }

        [Fact]
        public async Task HealthCheck_ReturnsTextContentType()
        {
            var response = await _client!.GetAsync("/Detector/HealthCheck");

            Assert.NotNull(response);
            Assert.NotNull(response.Content.Headers.ContentType);
        }

       

        
    }
}
