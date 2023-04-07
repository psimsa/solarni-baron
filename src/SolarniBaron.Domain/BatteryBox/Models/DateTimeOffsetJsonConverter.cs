using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models;

public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var pragueTimeZoneInfo = DateTimeHelpers.GetPragueTimeZoneInfo();
        
        var parsedDate = DateTime.ParseExact(reader.GetString()!,
            "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);

        TimeSpan pragueTimeZoneOffset = pragueTimeZoneInfo.GetUtcOffset(parsedDate);

        return new DateTimeOffset(parsedDate, pragueTimeZoneOffset);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeOffset dateTimeValue,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(dateTimeValue.ToString(
            "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
    }
}
