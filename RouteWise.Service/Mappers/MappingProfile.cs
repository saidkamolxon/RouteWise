using AutoMapper;
using RouteWise.Domain.Entities;
using RouteWise.Service.DTOs;

namespace RouteWise.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserCreationDto>().ReverseMap();
    }
}
