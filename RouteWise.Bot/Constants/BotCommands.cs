namespace RouteWise.Bot.Constants;

public class BotCommands
{
    public const string MeasureDistance = "/distance";
    public const string Start = "/start";
    public const string Help  = "/help";

    public static bool Contains(string command)
    {
        var fields = typeof(BotCommands)
            .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

        foreach (var field in fields)
            if (field.GetValue(null) as string == command)
                return true;

        return false;
    }
}
