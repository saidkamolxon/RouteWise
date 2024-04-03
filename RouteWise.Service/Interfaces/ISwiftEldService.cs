using RouteWise.Service.DTOs.Truck;

namespace RouteWise.Service.Interfaces;

public interface ISwiftEldService
{
    Task<IEnumerable<TruckStateDto>> GetAllTrucksStatesAsync();
    Task<TruckStateDto> GetTruckStateByNameAsync(string name);
}
