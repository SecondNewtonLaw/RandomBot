
using System;
using System.Collections.Generic;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace BibleAPI;

#pragma warning disable CS8618 // Disable possible null ref.

public partial class BibleJson
{
    [JsonPropertyName("reference")]
    public string Reference { get; set; }

    [JsonPropertyName("verses")]
    public Verse[] Verses { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("translation_id")]
    public string TranslationId { get; set; }

    [JsonPropertyName("translation_name")]
    public string TranslationName { get; set; }

    [JsonPropertyName("translation_note")]
    public string TranslationNote { get; set; }
}

public partial class Verse
{
    [JsonPropertyName("book_id")]
    public string BookId { get; set; }

    [JsonPropertyName("book_name")]
    public string BookName { get; set; }

    [JsonPropertyName("chapter")]
    public long Chapter { get; set; }

    [JsonPropertyName("verse")]
    public long VerseVerse { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public partial class BibleJson
{
    public static BibleJson FromJson(string json) => JsonSerializer.Deserialize<BibleJson>(json, BibleAPI.Converter.Settings);
}

public static class Serialize
{
    public static string ToJson(this BibleJson self) => JsonSerializer.Serialize(self, BibleAPI.Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.Web)
    {
        Converters =
            {
                new DateOnlyConverter(),
                new TimeOnlyConverter(),
            },
    };
}

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    private readonly string serializationFormat;
    public DateOnlyConverter() : this(null) { }

    public DateOnlyConverter(string? serializationFormat)
    {
        this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
    }

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return DateOnly.Parse(value!);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(serializationFormat));
}
public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    private readonly string serializationFormat;

    public TimeOnlyConverter() : this(null) { }

    public TimeOnlyConverter(string? serializationFormat)
    {
        this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
    }

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return TimeOnly.Parse(value!);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(serializationFormat));
}
