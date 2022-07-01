namespace Exertive.Core.Identification
{

    #region Dependencies

    using MongoDB.Bson;

    using System;
    using System.Collections.Generic;

    #endregion Dependencies

    public class IdentificationService
    {

        private static class Domains
        {
            internal static string Authentication = "Authentication";
            internal static string Administration = "Administration";
        }

        private readonly string _indexScheme = "https://";

        private readonly string _indexAuthority = "index.exertive.io";

        private readonly Uri _indexUri;

        private readonly GuidGenerator _guidGenerator = new();

        private readonly Dictionary<string, string> _locators = new()
        {
            { "Credentials", Domains.Authentication },
            { "Identity", Domains.Authentication },
            { "Role", Domains.Authentication },
            { "User", Domains.Administration },
        };


        public IdentificationService()
        {
            this._indexUri = new Uri(this._indexScheme + this._indexAuthority);
        }

        public Uri Locate(Type type)
        {
            return this.Locate(type.Name.Replace("Entity", String.Empty));
        }

        public Uri Locate(string model)
        {
            if (this._locators.ContainsKey(model))
            {
                var baseUri = new Uri(this._indexUri, this._locators[model].ToLowerInvariant());
                var finalUri = new Uri(baseUri.AbsoluteUri + "/" + model.ToLowerInvariant());
                return finalUri;
            }
            throw new ArgumentException(model, nameof(model));
        }

        /// <summary>
        /// Generates an Identification Key for the Type and string Identifier provided, where the
        /// type can either be a Model or an Entity type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Guid Identify(Type type, string id)
        {
            var baseUri = this.Locate(type);
            var uri = new Uri(baseUri.AbsoluteUri + "/" + id);
            return this._guidGenerator.GenerateUriGuid(uri.AbsoluteUri);
        }

        public Guid Identify(string model, string id)
        {
            var uri = new Uri(this.Locate(model), id);
            return this._guidGenerator.GenerateUriGuid(uri.AbsoluteUri);
        }

    }
}
