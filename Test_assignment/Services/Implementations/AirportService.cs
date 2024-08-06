using Test_assignment.Models;
using Test_assignment.Repositories.Implementations;
using Test_assignment.Repositories.Interfaces;
using Test_assignment.Services.Interfaces;

namespace Test_assignment.Services.Implementations
{
    public class AirportService : IAirportService
    {
        private readonly IAirportRepository _airportRepository;
        private readonly ILogger<AirportService> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AirportService"/>.
        /// </summary>
        /// <param name="airportRepository">Репозиторий для получения данных об аэропортах.</param>
        /// <param name="logger">Логгер для записи информации о запросах и ответах.</param>
        public AirportService(IAirportRepository airportRepository, ILogger<AirportService> logger)
        {
            _airportRepository = airportRepository;
            _logger = logger;
        }

        /// <summary>
        /// Получает информацию об аэропорте по его IATA-коду.
        /// </summary>
        /// <param name="iataCode">IATA-код аэропорта.</param>
        /// <returns>Информация об аэропорте.</returns>
        public async Task<Airport?> GetAirportByIataCode(string iataCode)
        {
            var airport = await _airportRepository.GetAirportAsync(iataCode);
            if (airport == null)
            {
                _logger.LogWarning("Failed to retrieve airport data for IATA code {IataCode}.", iataCode);
            }

            return airport;
        }
    }
}
