using RouteWise.Service.DTOs.User;

namespace RouteWise.Service.Interfaces;

public interface IUserService
{
    bool IsPermittedUser(long id);
    Task<UserResultDto> GetByTelegramIdAsync(long id);
    Task<UserResultDto> UpdateAsync(UserUpdateDto dto);
    Task<UserResultDto> AddAsync(UserCreationDto dto);
}