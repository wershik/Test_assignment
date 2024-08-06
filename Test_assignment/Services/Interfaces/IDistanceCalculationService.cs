using Test_assignment.Models;

namespace Test_assignment.Services.Interfaces
{
    public interface IDistanceCalculationService
    {
        double CalculateDistance(Airport airportFrom, Airport airportTo);
    }
}
