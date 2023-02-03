using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarniBaron.Domain;
public class DateTimeHelpers
{
    public static TimeZoneInfo GetPragueTimeZoneInfo() =>
        Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT => TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"),
            _ => TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague")
        };

    public static DateTime GetPragueDateTimeNow()
    {
        var pragueTimeZoneInfo = GetPragueTimeZoneInfo();
        var utcNow = DateTime.UtcNow;
        return TimeZoneInfo.ConvertTimeFromUtc(utcNow, pragueTimeZoneInfo);
    }
}
