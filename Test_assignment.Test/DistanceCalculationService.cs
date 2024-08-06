using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_assignment.Models;
using Test_assignment.Services.Implementations;

namespace Test_assignment.Test
{
    public class DistanceCalculationServiceTests
    {
        private readonly DistanceCalculationService _distanceCalculationService;

        public DistanceCalculationServiceTests()
        {
            _distanceCalculationService = new DistanceCalculationService();
        }

        [Fact]
        public void CalculateDistance_CorrectlyCalculatesDistanceInMiles()
        {
            // Arrange
            var airportFrom = new Airport
            {
                Location = new Location { Lat = 40.6413, Lon = -73.7781 } // JFK
            };
            var airportTo = new Airport
            {
                Location = new Location { Lat = 33.9416, Lon = -118.4085 } // LAX
            };

            // Act
            var distance = _distanceCalculationService.CalculateDistance(airportFrom, airportTo);

            // Assert
            Assert.InRange(distance, 2450, 2500); // Убедитесь, что расстояние в пределах допустимого диапазона
        }
    }
}
