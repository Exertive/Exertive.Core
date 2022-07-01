namespace Exertive.Core.Domain.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

#nullable enable

    public class IdentifierTypeConverter : JsonConverter<string?>
    {

        public override bool CanConvert(Type type)
        {
            return type == typeof(string);
        }

        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (!(value == null))
            {
                writer.WriteStringValue(value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }

    }

#nullable restore

}

