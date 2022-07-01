
namespace Exertive.Core.Domain.Models
{
    using System;

    using Exertive.Core.Domain.Entities;

    using MongoDB.Bson;

#nullable enable

    public interface IModel<TKey>
    {

        #region Interface Properties


        /// <summary>
        /// Get the Unique Identifier of the Model instance. Note that
        /// this can be null for newly created instances. The Identifier
        /// takes the form of a 24-character string representing the 
        /// hexadecimal ObjectId assigned to the corresponding Entity and
        /// persisted in the MongoDB collection.
        /// </summary>
        string? Id { get; }

        /// <summary>
        /// Get the Unique Key of the Model instance. This is a deterministic
        /// GUID constructed from the hashed value of the URI of the Model
        /// instance, where the SHA-1 hash algorithm is used.
        /// </summary>
        Guid? Key { get; }

        /// <summary>
        /// Get the Signature of the Model, which is in the form of the domain,
        /// subdomain and model type in a path structure. For example:
        /// "banking/bank/account".
        /// </summary>
        string? Signature { get; }

        #endregion Interface Properties

        #region Interface Methods

        /// <summary>
        /// Identify the Model instance by assigning the identifier provided and its derived key.
        /// </summary>
        /// <typeparam name="TModel">The typeof of Model represented by the current instance.</typeparam>
        /// <param name="identifier">The Identifier to assign to the instance where the Identifier is of type TKey.</param>
        /// <returns>The current Model instance for chaining.</returns>
        TModel? Identify<TModel>(string type, TKey identifier) where TModel : class, IModel<TKey>;

        /// <summary>
        /// Identify the Model instance by assigning the identifier provided and its derived key.
        /// </summary>
        /// <typeparam name="TModel">The typeof of Model represented by the current instance.</typeparam>
        /// <param name="identifier">The Identifier to assign to the instance where the Identifier is of type TKey.</param>
        /// <returns>The current Model instance for chaining.</returns>
        TModel? Identify<TModel>(string type, string? identifier) where TModel : class, IModel<TKey>;

        bool HasProperty(string key);

        string? GetProperty(string key);

        TModel? Clone<TModel>() where TModel : class, IModel<TKey>, new();

        TModel? FromEntity<TEntity, TModel>(TEntity entity) where TEntity : IEntity<TKey> where TModel : class, IModel<TKey>;

        TEntity? ToEntity<TEntity, TModel>() where TEntity : class, IEntity<TKey>, new() where TModel : IModel<TKey>;

        #endregion Interface Methods

    }
}

#nullable disable
