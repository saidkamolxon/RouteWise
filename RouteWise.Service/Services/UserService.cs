using AutoMapper;
using RouteWise.Data.IRepositories;
using RouteWise.Service.DTOs.User;
using RouteWise.Service.Exceptions;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserResultDto> GetByTelegramId(long id)
    {
        var user = await _unitOfWork.UserRepository
            .SelectAsync(u => u.TelegramId == id)
            ?? throw new NotFoundException("A user with such id is not found.");

        return _mapper.Map<UserResultDto>(user);
    }
}
