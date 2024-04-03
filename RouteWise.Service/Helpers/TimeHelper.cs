namespace RouteWise.Service.Helpers;

public static class TimeHelper
{
    public const string DefaultTimeZone = "Eastern Standard Time";

    public static DateTime ConvertUtcToDefaultTime(this DateTime utcDateTime)
        => TimeZoneInfo.ConvertTimeFromUtc(utcDateTime,
            TimeZoneInfo.FindSystemTimeZoneById(DefaultTimeZone));
}
