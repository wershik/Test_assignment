using Microsoft.AspNetCore.Mvc;
using Test_assignment.Models;
using Test_assignment.Services.Interfaces;

namespace Test_assignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistanceController : ControllerBase
    {
        private readonly IAirportService _airportService;
        private readonly IDistanceCalculationService _distanceCalculationService;
        private readonly ILogger<DistanceController> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DistanceController"/>.
        /// </summary>
        /// <param name="airportService">Сервис для получения информации об аэропортах. </param>
        /// <param name="distanceCalculationService">Сервис для расчета расстояний.</param>
        /// <param name="logger">Логгер для записи информации о запросах и ответах.</param>
        public DistanceController(IAirportService airportService, IDistanceCalculationService distanceCalculationService, ILogger<DistanceController> logger)
        {
            _airportService = airportService;
            _distanceCalculationService = distanceCalculationService;
            _logger = logger;
        }

        /// <summary>
        /// Получает расстояние между двумя аэропортами по их IATA-кодам.
        /// </summary>
        /// <param name="from">IATA-код аэропорта отправления.</param>
        /// <param name="to">IATA-код аэропорта назначения.</param>
        /// <returns>Расстояние между аэропортами в милях.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // Возвращает 200 OK при успешном запросе
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Возвращает 404, если один или оба аэропорта не найдены
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Возвращает 500, если произошла внутренняя ошибка сервера
        public async Task<IActionResult> GetDistance(string from, string to)
        {
            _logger.LogInformation("Received request to calculate distance from {From} to {To}.", from, to);

            var airportFrom = await _airportService.GetAirportByIataCode(from);
            var airportTo = await _airportService.GetAirportByIataCode(to);

            if (airportFrom == null)
            {
                _logger.LogWarning("Airport with IATA code '{From}' not found.", from);
                return NotFound($"Airport with IATA code '{from}' not found.");
            }

            if (airportTo == null)
            {
                _logger.LogWarning("Airport with IATA code '{To}' not found.", to);
                return NotFound($"Airport with IATA code '{to}' not found.");
            }

            var distance = _distanceCalculationService.CalculateDistance(airportFrom, airportTo);

            // Логирование отправки ответа
            _logger.LogInformation("Sending response: Distance between {From} and {To} is {Distance} miles.", from, to, distance);

            return Ok(new DistanceResult { Distance = distance });
        }
    }
}
