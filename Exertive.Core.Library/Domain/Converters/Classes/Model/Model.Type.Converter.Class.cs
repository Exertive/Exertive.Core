namespace Exertive.Core.Domain.Converters
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using Exertive.Core.Domain.Models;

    // ReSharper disable UnusedMember.Local
    public class ModelTypeConverter<TModel, TKey> : JsonConverter<TModel> where TModel : class, IModel<TKey>, new()
    {
        public override TModel Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            //TModel value = new TModel();
            //var success = JsonDocument.TryParseValue(ref reader, out JsonDocument document);
            //return value;
            var value = JsonSerializer.Deserialize(ref reader, type, options) as TModel;
            return value;
        }

        public override void Write(Utf8JsonWriter writer, TModel value, JsonSerializerOptions options)
        {
        }

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Possible use in the future")]
        private TModel PopulateObject(TModel target, string jsonSource)
        {
            return this.PopulateObject(target, jsonSource, typeof(TModel));
        }

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Possible use in the future")]
        private TModel OverwriteProperty(TModel target, JsonProperty updatedProperty)
        {
            return this.OverwriteProperty(target, updatedProperty, typeof(TModel));
        }

        private TModel PopulateObject(TModel target, string jsonSource, Type type)
        {
            var json = JsonDocument.Parse(jsonSource).RootElement;
            foreach (var property in json.EnumerateObject())
            {
                this.OverwriteProperty(target, property, type);
            }
            return target;
        }

        private TModel OverwriteProperty(TModel target, JsonProperty source, Type type)
        {
            var destination = type.GetProperty(source.Name);
            if (destination == null)
            {
                return target;
            }
            TModel value;
            if (destination.PropertyType.IsValueType)
            {
                value = JsonSerializer.Deserialize(source.Value.GetRawText(), destination.PropertyType) as TModel;
            }
            else
            {
                value = destination.GetValue(target) as TModel;
                this.PopulateObject(value, source.Value.GetRawText(), destination.PropertyType);
            }
            destination.SetValue(target, value);
            return target;
        }
    }
    // ReSharper restore UnusedMember.Local

}
