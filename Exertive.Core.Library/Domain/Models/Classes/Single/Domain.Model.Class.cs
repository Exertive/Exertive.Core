
namespace Exertive.Core.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.Json.Serialization;

    using Exertive.Core.Domain.Converters;
    using Exertive.Core.Domain.Entities;
    using Exertive.Core.Framework.Stores;
    using Exertive.Core.Identification;

    using MongoDB.Bson;

#nullable enable

    [Serializable]
    public abstract class Model<TKey> : IModel<TKey>
    {

        #region Protected Instance Properties

        protected readonly IStore<TKey>? Store;

        #endregion Protected Instance Properties

        #region Public Instance Properties

        [JsonPropertyName("id")]
        [JsonConverter(typeof(IdentifierTypeConverter))]
        public string? Id
        {
            get;
            set;
        }

        [JsonPropertyName("key")]
        [JsonConverter(typeof(KeyTypeConverter))]
        public Guid? Key
        {
            get;
            set;
        }

        [JsonPropertyName("signature")]
        public string? Signature
        {
            get;
            set;
        }

        #endregion Public Instance Properties

        #region Constructor

        public Model()
        {
            this.Signature = this.GetBaseName();
        }

        #endregion Constructor

        #region Public Static Methods

        /// <summary>
        /// Resolved the MongoDb ObjectId identifier provided to its string equivalent for assignment to a Model instance;
        /// or null if the identifier itself is null.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static string? ResolveIdentifier(TKey? identifier)
        {
            if (!(identifier == null))
            {
                return identifier.ToString();
            }
            return null;
        }

        public static TKey? ResolveIdentifier(string? identifier)
        {
            if (identifier is not null)
            {
                if (typeof(TKey) == typeof(ObjectId))
                {
                    return (TKey?)(ObjectId.Parse(identifier) as IEquatable<ObjectId>);
                }
            }
            return default;
        }

        public static Guid? ResolveKey(IModel<TKey> instance)
        {
            if (instance != null && instance.Key == null && instance.Key.HasValue && instance.Key != Guid.Empty)
            {
                return new Guid(instance.Key.Value.ToByteArray());
            }
            return null;
        }

        public static M? ResolveInstance<E, M>(IStore<TKey> store, string id) where E : class, IEntity<TKey>, new() where M : class, IModel<TKey>, new()
        {
            if (store != null && id != null)
            {
                // return store.Get<E, M>(id);
            }
            return null;
        }

        public static M? ResolveModel<M>(IModel<TKey> instance) where M : class, IModel<TKey>, new()
        {
            if (instance != null)
            {
                return instance.Clone<M>();
            }
            return null;
        }

        public static string? ResolveString(string instance)
        {
            if (instance != null)
            {
                return new String(instance);
            }
            return null;
        }

        public static IEnumerable<string>? ResolveCollection(IList<string> instance)
        {
            if (instance != null)
            {
                var length = instance.Count;
                var collection = new List<string>(length);
                if (length > 0)
                {
                    foreach (var member in instance)
                    {
                        collection.Add(member);
                    }
                }
                return collection as IEnumerable<string>;
            }
            return null;
        }

        public static IEnumerable<TEntity>? ResolveCollection<TEntity>(IList<TEntity> instance) where TEntity : Entity<TKey>, new()
        {
            if (instance != null)
            {
                var length = instance.Count;
                var collection = new List<TEntity>(length);
                if (length > 0)
                {
                    foreach (var member in instance)
                    {
                        collection.Add(member.Clone<TEntity>());
                    }
                }
                return collection as IEnumerable<TEntity>;
            }
            return null;
        }

        #endregion Public Static Methods

        #region Public Instance Methods

        public TModel? Identify<TModel>(string type, TKey? id) where TModel : class, IModel<TKey>
        {
            if (id is not null)
            {
                this.Id = id.ToString();
                this.Key = new IdentificationService().Identify(type, this.Id);
            }
            this.Signature = type;
            return this as TModel;
        }

        public TModel? Identify<TModel>(string type, string? id) where TModel : class, IModel<TKey>
        {
            if (id is not null)
            {
                this.Id = id;
                this.Key = new IdentificationService().Identify(type, this.Id);
            }
            this.Signature = type;
            return this as TModel;
        }

        public virtual bool HasProperty(string key)
        {
            var type = this.GetType();
            var property = type.GetProperty(key, BindingFlags.Instance | BindingFlags.Public);
            return property != null;
        }

        public virtual string? GetProperty(string key)
        {
            var type = this.GetType();
            var property = type.GetProperty(key, BindingFlags.Instance | BindingFlags.Public);
            return property != null ? property.GetValue(this) as string : null;
        }

        public abstract TModel? Clone<TModel>() where TModel : class, IModel<TKey>, new();

        public virtual TModel? FromEntity<TEntity, TModel>(TEntity entity) where TEntity : IEntity<TKey> where TModel : class, IModel<TKey>
        {
            this.Id = (entity.Id?.ToString());
            return this as TModel;
        }

        public virtual TEntity? ToEntity<TEntity, TModel>() where TEntity : class, IEntity<TKey>, new() where TModel : IModel<TKey>
        {
            IEntity<TKey> entity = new TEntity();
            return entity as TEntity;
        }

        #endregion Public Instance Methods

        #region Private Instance Methods

        private string GetBaseName()
        {
            var type = this.GetType();
            return type.IsGenericType ? type.Name[..type.Name.IndexOf('`')] : type.Name;
        }

        #endregion Private Instance Methods

    }

#nullable restore

}

