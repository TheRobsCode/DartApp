using Dart;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using Xunit;

namespace DartTests
{
    /// <summary>
    /// Integration tests that call the real Irish Rail API.
    /// These tests may fail if the network is unavailable or the API is down.
    /// Mark with [Fact(Skip = "Integration test")] if you want to skip them in CI.
    /// </summary>
    public class DartServiceIntegrationTests
    {
        private readonly IDartService _dartService;

        public DartServiceIntegrationTests()
        {
            var fakeLogger = A.Fake<ILogger<DartService>>();
            _dartService = new DartService(fakeLogger);
        }

        [Fact(Skip = "Integration test - requires network")]
        public async Task GetStations_ShouldReturnStations()
        {
            // Act
            var result = await _dartService.GetStations();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            // Verify some known stations exist
            Assert.Contains(result, s => s.StationDesc.Contains("Connolly"));
            Assert.Contains(result, s => s.StationDesc.Contains("Tara"));
            Assert.Contains(result, s => s.StationDesc.Contains("Pearse"));
        }

        [Fact(Skip = "Integration test - requires network")]
        public async Task GetTimeTable_ValidStation_ShouldReturnData()
        {
            // Arrange
            const string station = "Connolly";

            // Act
            var result = await _dartService.GetTimeTable(station);

            // Assert
            Assert.NotNull(result);
            // Note: Result might be empty if no trains are scheduled
        }
    }
}
