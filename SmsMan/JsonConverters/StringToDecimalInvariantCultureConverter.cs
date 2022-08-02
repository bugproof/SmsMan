using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmsMan.JsonConverters;

public class StringToDecimalInvariantCultureConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return decimal.Parse(reader.GetString() ?? throw new InvalidOperationException(), CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}