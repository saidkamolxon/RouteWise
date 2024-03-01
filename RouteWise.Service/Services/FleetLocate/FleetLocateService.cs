using System.Text;

namespace RouteWise.Service.Services.FleetLocate;

public class FleetLocateService
{
    private readonly byte[] _user;
    public FleetLocateService(string login, string password, string accountId)
    {
        _user = Encoding.ASCII.GetBytes($"{login}:{password}");
    }
}
