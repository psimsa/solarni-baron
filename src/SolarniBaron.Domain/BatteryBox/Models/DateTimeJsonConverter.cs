using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models;

public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var pragueTimeZoneInfo = DateTimeHelpers.GetPragueTimeZoneInfo();
        var currentTimeZoneInfo = TimeZoneInfo.Local;
        var parsedDate = DateTime.ParseExact(reader.GetString()!,
            "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        var totalOffset = currentTimeZoneInfo.BaseUtcOffset - pragueTimeZoneInfo.BaseUtcOffset;
        return parsedDate.AddMinutes(totalOffset.TotalMinutes);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTime dateTimeValue,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(dateTimeValue.ToString(
            "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
    }
}
