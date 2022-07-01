namespace Exertive.Core.Domain.Converters
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    // ReSharper disable MemberCanBeMadeStatic.Local
    public class ConcreteTypeConverter<T> : JsonConverter<T> where T : class
    {
        public override bool CanConvert(Type type)
        {
            return true;
        }

        public override T Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            var converter = this.GetConcreteConverter(options);
            return converter.Read(ref reader, type, options) as T;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var converter = this.GetConcreteConverter(options);
            converter.Write(writer, value, options);
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        private JsonConverter<T> GetConcreteConverter(JsonSerializerOptions options)
        {
            if (options.GetConverter(typeof(T)) is not JsonConverter<T> converter)
            {
                throw new JsonException("Missing JSON Converter for concrete type '" + typeof(T).Name + "'.");
            }
            return converter;
        }
    }
    // ReSharper restore MemberCanBeMadeStatic.Local

}
