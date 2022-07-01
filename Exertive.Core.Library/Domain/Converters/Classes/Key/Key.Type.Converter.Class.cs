namespace Exertive.Core.Domain.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class KeyTypeConverter : JsonConverter<Guid>
    {
        public override bool CanConvert(Type type)
        {
            return type == typeof(Guid);
        }

        public override Guid Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            var guid = Guid.Empty;
            var value = reader.GetString();
            if (!String.IsNullOrEmpty(value))
            {
                guid = Guid.Parse(value);
            }
            return guid;
        }

        public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
        {
            var guid = value;
            if (guid != Guid.Empty)
            {
                writer.WriteStringValue(guid.ToString("D"));
            }
            else
            {
                writer.WriteNullValue();
            }
        }

    }
}
