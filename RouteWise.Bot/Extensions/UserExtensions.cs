namespace RouteWise.Bot.Extensions;

public static class UserExtensions
{
    public static string GetFullName(this Telegram.Bot.Types.User user)
    {
        return $"{user.FirstName} {user.LastName}";
    }
}
