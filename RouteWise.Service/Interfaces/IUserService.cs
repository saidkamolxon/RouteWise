using RouteWise.Service.DTOs.User;

namespace RouteWise.Service.Interfaces;

public interface IUserService
{
    Task<UserResultDto> GetByTelegramId(long id);
}
