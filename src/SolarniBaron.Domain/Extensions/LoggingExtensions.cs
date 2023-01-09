using Microsoft.Extensions.Logging;

namespace SolarniBaron.Domain.Extensions;

public static partial class LoggingExtensions
{
    [LoggerMessage(0, LogLevel.Information, "Cache not hit. Fetching stats from BatteryBox...")]
    public static partial void LogCacheNotHit(this ILogger logger);

    [LoggerMessage(1, LogLevel.Information, "Fetching stats from BatteryBox...")]
    public static partial void LogFetchingStatsFromBatteryBox(this ILogger logger);

    [LoggerMessage(2, LogLevel.Information, "Stats fetched.")]
    public static partial void LogStatsFetched(this ILogger logger);

    [LoggerMessage(3, LogLevel.Information, "Stats retrieved from cache.")]
    public static partial void LogCacheHit(this ILogger logger);

    [LoggerMessage(100, LogLevel.Debug,
        "Date info: lastCallDate={lastCallDate}, nextRefresh={nextRefresh}, cacheExpire={cacheExpireOffset}")]
    public static partial void LogDateInfo(this ILogger logger, DateTimeOffset lastCallDate, TimeSpan nextRefresh,
        DateTimeOffset cacheExpireOffset);

    [LoggerMessage(200, LogLevel.Warning, "Empty body in POST request")]
    public static partial void LogEmptyBodyInPostRequest(this ILogger logger);

    [LoggerMessage(300, LogLevel.Error, "User not authorized")]
    public static partial void LogUserNotAuthorized(this ILogger logger);
}
