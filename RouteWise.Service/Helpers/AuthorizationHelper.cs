using System.Text;

namespace RouteWise.Service.Helpers;

public static class AuthorizationHelper
{
    public static string GetAuthString(string login, string password)
    {
        byte[] userBytes = Encoding.ASCII.GetBytes($"{login}:{password}");
        return Convert.ToBase64String(userBytes);
    }
}
