using AutoMapper;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
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

    public bool IsPermittedUser(long id)
        => _unitOfWork.UserRepository
            .SelectAll()
            .Any(u => u.TelegramId == id);

    public async Task<UserResultDto> GetByTelegramIdAsync(long id,  CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository
            .SelectAsync(u => u.TelegramId == id) ??
                throw new NotFoundException("A user with such id is not found.");

        return _mapper.Map<UserResultDto>(user);
    }

    public async Task<UserResultDto> UpdateAsync(UserUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(u => u.Id == dto.Id)
            ?? throw new NotFoundException($"User with such id={dto.Id} is not found");

        _mapper.Map(dto, user);
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(user);
    }

    public async Task<UserResultDto> AddAsync(UserCreationDto dto, CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<User>(dto);
        await _unitOfWork.UserRepository.CreateAsync(user);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<UserResultDto>(user);
    }
}
