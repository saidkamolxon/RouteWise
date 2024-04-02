using System.Runtime.CompilerServices;

namespace RouteWise.Service.Extensions;

public static class CustomDateTimeExtension
{
    public static DateTime ConvertUtcToEdt(this DateTime utcDateTime)
        => TimeZoneInfo.ConvertTimeFromUtc(utcDateTime,
            TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
}
