using Test_assignment.Models;

namespace Test_assignment.Services.Interfaces
{
    public interface IAirportService
    {
        Task<Airport> GetAirportByIataCode(string iataCode);
    }
}
