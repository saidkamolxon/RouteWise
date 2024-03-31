using AutoMapper;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Newtonsoft.Json.Linq;
using RouteWise.Domain.Entities;
using RouteWise.Service.DTOs;
using RouteWise.Service.DTOs.Landmark;
using RouteWise.Service.DTOs.Trailer;

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
        CreateMap<LandmarkUpdateDto, Landmark>();
        CreateMap<Landmark, LandmarkResultDto>();
    }
}
