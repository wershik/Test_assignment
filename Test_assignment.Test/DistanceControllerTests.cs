using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_assignment.Controllers;
using Test_assignment.Models;
using Test_assignment.Services.Interfaces;

namespace Test_assignment.Test
{
    public class DistanceControllerTests
    {
        private readonly Mock<IAirportService> _mockAirportService;
        private readonly Mock<IDistanceCalculationService> _mockDistanceCalculationService;
        private readonly Mock<ILogger<DistanceController>> _mockLogger;
        private readonly DistanceController _controller;

        public DistanceControllerTests()
        {
            _mockAirportService = new Mock<IAirportService>();
            _mockDistanceCalculationService = new Mock<IDistanceCalculationService>();
            _mockLogger = new Mock<ILogger<DistanceController>>();
            _controller = new DistanceController(
                _mockAirportService.Object,
                _mockDistanceCalculationService.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task GetDistance_ReturnsNotFound_WhenAirportFromIsNotFound()
        {
            // Arrange
            _mockAirportService.Setup(s => s.GetAirportByIataCode("XXX")).ReturnsAsync((Airport?)null);
            _mockAirportService.Setup(s => s.GetAirportByIataCode("LAX")).ReturnsAsync(new Airport { Iata = "LAX" });

            // Act
            var result = await _controller.GetDistance("XXX", "LAX");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Airport with IATA code 'XXX' not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetDistance_ReturnsNotFound_WhenAirportToIsNotFound()
        {
            // Arrange
            _mockAirportService.Setup(s => s.GetAirportByIataCode("JFK")).ReturnsAsync(new Airport { Iata = "JFK" });
            _mockAirportService.Setup(s => s.GetAirportByIataCode("XXX")).ReturnsAsync((Airport?)null);

            // Act
            var result = await _controller.GetDistance("JFK", "XXX");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Airport with IATA code 'XXX' not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetDistance_ReturnsOkResult_WhenAirportsAreFound()
        {
            // Arrange
            var airportFrom = new Airport { Iata = "JFK", Location = new Location { Lat = 40.6413, Lon = -73.7781 } };
            var airportTo = new Airport { Iata = "LAX", Location = new Location { Lat = 33.9416, Lon = -118.4085 } };

            _mockAirportService.Setup(s => s.GetAirportByIataCode("JFK")).ReturnsAsync(airportFrom);
            _mockAirportService.Setup(s => s.GetAirportByIataCode("LAX")).ReturnsAsync(airportTo);
            _mockDistanceCalculationService.Setup(s => s.CalculateDistance(airportFrom, airportTo)).Returns(2475);

            // Act
            var result = await _controller.GetDistance("JFK", "LAX");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<DistanceResult>(okResult.Value); // Используем конкретный класс

            Assert.Equal(2475, returnValue.Distance);
        }
    }
}
