using System.ComponentModel.DataAnnotations;
using RouteWise.Domain.Enums;
using RouteWise.Domain.Models;

namespace RouteWise.Domain.Entities;

public class User : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public long TelegramId { get; set; }
    public UserRole Role { get; set; } = UserRole.User;

    public Step CurrentStep { get; set; } = Step.Initial;
    
    [Required]
    public StepValue StepValue { get; set; }
}