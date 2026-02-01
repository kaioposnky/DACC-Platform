using System.Net;
using System.Net.Http.Json;
using DaccApi.Model.Responses.Statistics;
using DaccApi.Responses;
using FluentAssertions;
using DaccApi.Tests;
using Xunit;

namespace DaccApi.Tests.Statistics
{
    public class StatisticsControllerTests : IntegrationTestBase
    {
        [Fact]
        public async Task Get_Dashboard_Stats_Should_Return_Data_For_Admin()
        {
            // Arrange
            await AuthenticateAsAdminAsync(); 

            // Act
            var response = await _client.GetAsync("/v1/api/statistics/dashboard");

            // Assert
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = await response.Content.ReadAsStringAsync();
                Assert.Fail($"Request failed with status {response.StatusCode}. Response: {error}");
            }
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadFromJsonAsync<TestResponse<ResponseDashboardStats>>();
            content.Should().NotBeNull();
            content!.Data.Should().NotBeNull();

            // Basic checks
            content.Data.Users.Should().NotBeNull();
            content.Data.Orders.Should().NotBeNull();
            content.Data.Products.Should().NotBeNull();
        }
    }

    public class TestResponse<T>
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
