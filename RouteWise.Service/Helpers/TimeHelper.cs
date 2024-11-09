namespace RouteWise.Service.Helpers;

public static class TimeHelper
{
    public const string DefaultTimeZone = "Eastern Standard Time";

    public static DateTime ConvertUtcToDefaultTime(this DateTime utcDateTime)
        => TimeZoneInfo.ConvertTimeFromUtc(utcDateTime,
            TimeZoneInfo.FindSystemTimeZoneById(DefaultTimeZone));
    public static string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalMinutes < 60) return $"{duration.TotalMinutes:0}m";
        if (duration.TotalHours < 24) return $"{duration.TotalHours:0}h";
        return $"{duration.TotalDays:0}d";
    }

    public static string FormatDuration(DateTime from, DateTime to)
        => FormatDuration(to - from);
}
