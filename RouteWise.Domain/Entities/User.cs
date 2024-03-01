using RouteWise.Domain.Enums;

namespace RouteWise.Domain.Entities;

public class User : Auditable
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public long TelegramId { get; set; } // User's telegram id
    public UserRole Role { get; set; } = UserRole.User;
}
