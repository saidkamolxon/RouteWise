using RouteWise.Domain.Enums;

namespace RouteWise.Domain.Entities;

public class User : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public long TelegramId { get; set; } // User's telegram id
    public UserRole Role { get; set; } = UserRole.User;
}
