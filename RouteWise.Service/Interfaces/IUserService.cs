using RouteWise.Service.DTOs.User;

namespace RouteWise.Service.Interfaces;

public interface IUserService
{
    bool IsPermittedUser(long id);
    Task<UserResultDto> GetByTelegramIdAsync(long id, CancellationToken cancellationToken = default);
    Task<UserResultDto> UpdateAsync(UserUpdateDto dto, CancellationToken cancellationToken = default);
    Task<UserResultDto> AddAsync(UserCreationDto dto, CancellationToken cancellationToken = default);
}