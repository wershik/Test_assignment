using GeoCoordinatePortable;
using Test_assignment.Models;
using Test_assignment.Services.Interfaces;

namespace Test_assignment.Services.Implementations
{
    public class DistanceCalculationService : IDistanceCalculationService
    {
        /// <summary>
        /// Рассчитывает расстояние между двумя аэропортами.
        /// </summary>
        /// <param name="airportFrom">Аэропорт отправления.</param>
        /// <param name="airportTo">Аэропорт назначения.</param>
        /// <returns>Расстояние между аэропортами в милях.</returns>
        public double CalculateDistance(Airport airportFrom, Airport airportTo)
        {
            var coord1 = new GeoCoordinate(airportFrom.Location.Lat, airportFrom.Location.Lon);
            var coord2 = new GeoCoordinate(airportTo.Location.Lat, airportTo.Location.Lon);

            double distanceInMeters = coord1.GetDistanceTo(coord2);
            double distanceInKilometers = distanceInMeters / 1000.0; // Конвертируем метры в километры
            double distanceInMiles = distanceInKilometers * 0.621371; // Конвертируем километры в мили

            return distanceInMiles;
        }
    }
}
