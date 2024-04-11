using AutoMapper;
using RouteWise.Domain.Entities;
using RouteWise.Service.DTOs.Landmark;
using RouteWise.Service.DTOs.Trailer;
using RouteWise.Service.DTOs.Truck;
using RouteWise.Service.DTOs.User;

namespace RouteWise.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserCreationDto, User>();
        CreateMap<Trailer, TrailerResultDto>();
        CreateMap<Trailer, TrailerWithinLandmarkDto>();
        CreateMap<TrailerCreationDto, Trailer>();
        CreateMap<TrailerStateDto, Trailer>();
        CreateMap<TruckStateDto, Truck>();
        CreateMap<TruckStateDto, TruckResultDto>();
        CreateMap<LandmarkUpdateDto, Landmark>();
        CreateMap<Landmark, LandmarkResultDto>();
    }
}
