namespace SolarniBaron.Domain;

public class DateTimeHelpers
{
    public static TimeZoneInfo GetPragueTimeZoneInfo() => TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");

    public static DateTimeOffset GetPragueDateTimeNow()
    {
        var pragueTimeZone = GetPragueTimeZoneInfo();
        return TimeZoneInfo.ConvertTime(DateTimeOffset.Now, pragueTimeZone);
    }
}
