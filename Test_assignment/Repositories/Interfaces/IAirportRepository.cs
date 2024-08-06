using Test_assignment.Models;

namespace Test_assignment.Repositories.Interfaces
{
    public interface IAirportRepository
    {
        Task<Airport?> GetAirportAsync(string iataCode);
    }
}
