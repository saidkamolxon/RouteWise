using RouteWise.Domain.Enums;
using RouteWise.Domain.Models;

namespace RouteWise.Service.DTOs.User;

public class UserUpdateDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public long TelegramId { get; set; }
    public UserRole Role { get; set; }
    public Step CurrentStep { get; set; }
    public StepValue StepValue { get; set; }
}