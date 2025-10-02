using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using Team_06.Data;
using Team_06.DTOs;
using Xunit;

namespace Team_06.Tests.Controllers
{
    public class ToolsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ToolsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Add in-memory database for testing
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabase");
                    });
                });
            });
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task RegisterTool_ShouldReturnCreatedTool()
        {
            // Arrange
            var createToolDto = new CreateToolDto
            {
                FriendlyName = "Test Hammer",
                Description = "A reliable claw hammer",
                Make = "Stanley",
                Model = "16oz",
                Category = "Hand Tools",
                Supplier = "Tool Store",
                PurchaseDate = DateTime.Now.AddDays(-30),
                UnitCost = 25.99m,
                Currency = "USD",
                QRData = "QR_TEST_001"
            };

            var json = JsonConvert.SerializeObject(createToolDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/tools/register", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var tool = JsonConvert.DeserializeObject<ToolDto>(responseContent);
            
            tool.Should().NotBeNull();
            tool!.FriendlyName.Should().Be("Test Hammer");
            tool.Make.Should().Be("Stanley");
            tool.Id.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetToolsByName_ShouldReturnMatchingTools()
        {
            // Arrange - First register a tool
            var createToolDto = new CreateToolDto
            {
                FriendlyName = "Screwdriver Set",
                Description = "Professional screwdriver set",
                Make = "Craftsman",
                Category = "Hand Tools",
                QRData = "QR_TEST_002"
            };

            var json = JsonConvert.SerializeObject(createToolDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/tools/register", content);

            // Act
            var response = await _client.GetAsync("/api/tools/search/name/Screwdriver");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var tools = JsonConvert.DeserializeObject<List<ToolDto>>(responseContent);
            
            tools.Should().NotBeNull();
            tools!.Should().HaveCountGreaterThan(0);
            tools.First().FriendlyName.Should().Contain("Screwdriver");
        }

        [Fact]
        public async Task GetAllTools_ShouldReturnAllTools()
        {
            // Act
            var response = await _client.GetAsync("/api/tools");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var tools = JsonConvert.DeserializeObject<List<ToolDto>>(responseContent);
            
            tools.Should().NotBeNull();
        }

        [Fact]
        public async Task SearchTools_ShouldReturnMatchingResults()
        {
            // Arrange - Register a tool first
            var createToolDto = new CreateToolDto
            {
                FriendlyName = "Electric Drill",
                Description = "Cordless electric drill",
                Make = "DeWalt",
                Category = "Power Tools",
                QRData = "QR_TEST_003"
            };

            var json = JsonConvert.SerializeObject(createToolDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/tools/register", content);

            // Act
            var response = await _client.GetAsync("/api/tools/search?searchTerm=Electric");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var tools = JsonConvert.DeserializeObject<List<ToolDto>>(responseContent);
            
            tools.Should().NotBeNull();
            tools!.Should().HaveCountGreaterThan(0);
        }
    }
}