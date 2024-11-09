using AutoMapper;

namespace RouteWise.Service.Interfaces;

public interface IConfiguredMappers
{
    IMapper FleetLocateLandmarkUpdateMapper { get; }
    IMapper FleetLocateTrailerStateMapper { get; }
    IMapper RoadReadyMapper { get; }
    IMapper SamsaraMapper { get; }
    IMapper SwiftEldMapper { get; }
}
