using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Domain.Entities;
using RouteWise.Service.DTOs;
using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserCreationDto, User>();
        CreateMap<Trailer, TrailerResultDto>();
        CreateMap<TrailerCreationDto, Trailer>();

        // 
        CreateMap<JObject, TrailerStateDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src["trailerName"].ToString()))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src["location"].ToString()))
            .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => $"{src["latitude"]},{src["longitude"]}"))
            .ForMember(dest => dest.LastEventDate, opt => opt.MapFrom(src => DateTime.Parse(src["lastEvent"]["messageDate"].ToString())))
            .ForMember(dest => dest.IsMoving, opt => opt.MapFrom(src => src["landmarkTrailerState"].ToString().Equals("InMotion")))
            .ForMember(dest => dest.LandmarkId, opt => opt.MapFrom(src => 1));
    }
}
