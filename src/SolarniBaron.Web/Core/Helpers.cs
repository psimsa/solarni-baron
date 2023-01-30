namespace SolarniBaron.Web.Core;

public static class Helpers
{
    public static string FormatMetric(decimal value, string baseUnit) => value switch
    {
        >= 1_000_000_000 => $"{value / 1000000000:0.##} G{baseUnit}",
        >= 1_000_000 => $"{value / 1000000:0.##} M{baseUnit}",
        >= 1_000 => $"{value / 1000:0.##} k{baseUnit}",
        _ => $"{value} {baseUnit}",
    };

    public static string GetGradient(decimal current, decimal max)
    {
        var percent = Math.Min(current / max * 100, 100);
        return
            $"background-image: linear-gradient(90deg, rgb(5, 39, 103) 0%, #ffffff {percent}%); color: black; text-shadow: -1px -1px 0 #ffffff, 1px -1px 0 #ffffff, -1px 1px 0 #ffffff, 1px 1px 0 #ffffff;";
    }
}
