namespace RouteWise.Bot.Constants;

public class BotCommands
{
    public const string LandmarkStatus = "/lane_info";
    public const string TrailerStatusGoogle = "/g";
    public const string TrailerStatusBing = "/b";
    public const string MeasureDistance = "/distance";
    public const string AllTrailersInfo = "/all_trl_info";
    public const string Start = "/start";
    public const string Help  = "/help";
    public const string TruckList = "/truck_list";
    public const string TruckListWithoutDrivers = "/truck_list_driverless";
    public const string UnitDocuments = "/docs";
    public const string EtaToDestination = "/eta";
    public const string UpdateLandmarks = "/update_landmarks";

    public static bool Contains(string command)
    {
        var fields = typeof(BotCommands)
            .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        
        return fields.Any(field => field.GetValue(null) as string == command);
    }
}
