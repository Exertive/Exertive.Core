namespace Exertive.Core.Domain.Converters
{

    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using Exertive.Core.Domain.Models;

    public class ModelTypeConverterFactory<TModel, TKey> : JsonConverterFactory where TModel : class, IModel<TKey>, new()
    {
        public override bool CanConvert(Type type)
        {
            return type.BaseType == typeof(Model<TKey>);
        }

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
        {
            var converter = typeof(ModelTypeConverter<TModel, TKey>);

            return (JsonConverter)Activator.CreateInstance(converter);
        }
    }
}
