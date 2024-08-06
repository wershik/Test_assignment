using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Test_assignment.Configurations;
using Test_assignment.Models;
using Test_assignment.Repositories.Interfaces;

namespace Test_assignment.Repositories.Implementations
{
    public class AirportRepository : IAirportRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly ILogger<AirportRepository> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AirportRepository"/>.
        /// </summary>
        /// <param name="httpClient">HTTP-клиент для выполнения запросов к API.</param>
        /// <param name="apiSettings">Настройки API для получения данных.</param>
        public AirportRepository(HttpClient httpClient, IOptions<ApiSettings> apiSettings, ILogger<AirportRepository> logger)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Получает информацию об аэропорте по его IATA-коду.
        /// </summary>
        /// <param name="iataCode">IATA-код аэропорта.</param>
        /// <returns>Информация об аэропорте.</returns>
        /// <exception cref="InvalidOperationException">Возникает, если данные об аэропорте не были получены.</exception>
        public async Task<Airport?> GetAirportAsync(string iataCode)
        {
            try
            {
                var requestUrl = $"{_apiSettings.AirportApiBaseUrl}{iataCode}";
                var response = await _httpClient.GetStringAsync(requestUrl);

                var airport = JsonConvert.DeserializeObject<Airport>(response);

                if (airport == null || string.IsNullOrEmpty(airport.Iata))
                {
                    _logger.LogWarning("Airport with IATA code {IataCode} not found.", iataCode);
                    return null;
                }

                return airport;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching airport with IATA code {IataCode}.", iataCode);
                return null;
            }
        }
    }
}
