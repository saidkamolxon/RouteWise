namespace RouteWise.Bot.Constants;

public class BotCommands
{
    public const string GetLandmarkStatus = "/lane_info";
    public const string GetTrailerStatus = "/g";
    public const string MeasureDistance = "/distance";
    public const string GetAllTrailersInfo = "/all_trl_info";
    public const string Start = "/start";
    public const string Help  = "/help";

    public static bool Contains(string command)
    {
        var fields = typeof(BotCommands)
            .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

        return fields.Any(field => field.GetValue(null) as string == command);
    }
}
