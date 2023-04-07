namespace SolarniBaron.Domain;

public class DateTimeHelpers
{
    public static TimeZoneInfo GetPragueTimeZoneInfo()
    {
        return Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT => TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"),
            _ => TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague")
        };
    }

    public static DateTimeOffset GetPragueDateTimeNow()
    {
        var pragueTimeZone = GetPragueTimeZoneInfo();
        return TimeZoneInfo.ConvertTime(DateTimeOffset.Now, pragueTimeZone);
    }
}
