using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_assignment.Models;
using Test_assignment.Repositories.Interfaces;
using Test_assignment.Services.Implementations;

namespace Test_assignment.Test
{
    public class AirportServiceTests
    {
        private readonly Mock<IAirportRepository> _mockAirportRepository;
        private readonly Mock<ILogger<AirportService>> _mockLogger;
        private readonly AirportService _airportService;

        public AirportServiceTests()
        {
            _mockAirportRepository = new Mock<IAirportRepository>();
            _mockLogger = new Mock<ILogger<AirportService>>();
            _airportService = new AirportService(_mockAirportRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAirportByIataCode_ReturnsAirport_WhenAirportExists()
        {
            // Arrange
            var airport = new Airport { Iata = "JFK" };
            _mockAirportRepository.Setup(r => r.GetAirportAsync("JFK")).ReturnsAsync(airport);

            // Act
            var result = await _airportService.GetAirportByIataCode("JFK");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("JFK", result.Iata);
        }

        [Fact]
        public async Task GetAirportByIataCode_ReturnsNull_WhenAirportDoesNotExist()
        {
            // Arrange
            _mockAirportRepository.Setup(r => r.GetAirportAsync("XXX")).ReturnsAsync((Airport?)null);

            // Act
            var result = await _airportService.GetAirportByIataCode("XXX");

            // Assert
            Assert.Null(result);
        }
    }
}
