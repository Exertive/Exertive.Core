namespace Exertive.Core.Domain.Entities
{

    #region Dependencies

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    using Exertive.Core.Identification;
    using Exertive.Core.Framework.Stores;
    using Exertive.Core.Domain.Models;

    #endregion Dependencies


#nullable enable

    public abstract class Entity<TKey> : IEntity<TKey>
    {

        protected readonly IStore<TKey> Store;

        #region Public Properties

        [BsonId]
        [BsonRequired]
        [BsonElement("id")]
        public TKey? Id
        {
            get;
            set;
        }

        [BsonRequired]
        [BsonElement("key")]
        public string? Key
        {
            get;
            set;
        }

        [BsonRequired]
        [BsonElement("type")]
        public string? Type
        {
            get;
            set;
        }


        #endregion Public Properties

        #region Constructor

        public Entity(string type, IStore<TKey> store)
        {
            this.Type = type;
            this.Store = store;
        }

        #endregion Constructor

        #region Public Static Methods

        /// <summary>
        /// Resolves the MongoDb ObjectId identifier provided to its string equivalent for assignment to a Model instance;
        /// or null if the identifier itself is null.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static TKey? ResolveIdentifier(TKey? identifier)
        {
            if (identifier is not null)
            {
                return identifier;
            }
            return default;
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
            if (instance is not null && instance.Key is not null && instance.Key.HasValue && instance.Key != Guid.Empty)
            {
                return new Guid(instance.Key.Value.ToByteArray());
            }
            return default;
        }

        public static string? ResolveType(string? type)
        {
            if (type is not null)
            {
                return type;
            }
            return default;
        }


        public static M? ResolveInstance<E, M>(IStore<TKey> store, string id) where E : class, IEntity<TKey>, new() where M : class, IModel<TKey>, new()
        {
            if (store is not null && id is not null)
            {
                // return store.GetAsync<E, M>(id);
            }
            return default;
        }

        public static M? ResolveModel<M>(IModel<TKey> instance) where M : class, IModel<TKey>, new()
        {
            if (instance is not null)
            {
                return instance.Clone<M>();
            }
            return default;
        }

        public static string? ResolveString(string instance)
        {
            if (instance != null)
            {
                return new String(instance);
            }
            return null;
        }

        public static IList<T>? ResolveList<T>(IList<T> list)
        {
            if (list is not null)
            {
                return new List<T>(list.Select((entry) => entry));
            }
            return null;
        }

        public static IList<TEntity>? ResolveList<TEntity, TModel>(IList<TModel> list)
            where TEntity : class, IEntity<TKey>, new()
            where TModel : class, IModel<TKey>, new()
        {
            if (list is not null)
            {
                return new List<TEntity>(list.Select((entry) => entry.ToEntity<TEntity, TModel>()!));
            }
            return null;
        }

        public static ICollection<T>? ResolveCollection<T>(ICollection<T> collection)
        {
            if (collection is not null)
            {
                return new Collection<T>((collection.Select((entry) => entry) as IList<T>)!);
            }
            return null;
        }

        public static ICollection<TEntity>? ResolveCollection<TEntity, TModel>(ICollection<TModel> collection)
            where TEntity : class, IEntity<TKey>, new()
            where TModel : class, IModel<TKey>, new()
        {
            if (collection is not null)
            {
                return new Collection<TEntity>((collection.Select((entry) => entry.ToEntity<TEntity, TModel>()!) as IList<TEntity>)!);
            }
            return null;
        }


        #endregion Public Static Methods

        #region Public Instance Methods

        public virtual IEntity<TKey> Identify(TKey? id)
        {
            if (id is not null)
            {
                this.Id = id;
                this.Key = new IdentificationService().Identify(this.Type, id.ToString()).ToString();
            }
            return this;
        }

        public virtual IEntity<TKey> Identify(string? id)
        {
            this.Id = this.Parse(id);
            return this;
        }

        public abstract TEntity Clone<TEntity>() where TEntity : class, IEntity<TKey>, new();

        public virtual TEntity FromModel<TEntity, TModel>(TModel model) where TEntity : class, IEntity<TKey>, new() where TModel : IModel<TKey>
        {
            return (this.Identify(model.Id) as TEntity)!;
        }

        public virtual TModel ToModel<TEntity, TModel>() where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new()
        {
            return (new TModel().Identify<TModel>(this.Type!, this.Id!))!;
        }

        public virtual object? GetValue(string key)
        {
            try
            {
                var property = this.GetType().GetProperty(key);
                var value = null as object;
                if (property != null)
                {
                    value = property.GetValue(this, null);
                }
                else
                {
                    Debug.WriteLine("Get Entity Property Value: Attempt failed for " + this.GetType().Name + " class and " + key + " property.");
                }
                return value;
            }
            catch (Exception exception)
            {
                throw new Exception("Get Entity Property Value: Exception thrown.", exception);
            }
        }

        public static bool IsEmptyGuid(Guid value)
        {
            return value == Guid.Empty;
        }

        #endregion Public Instance Methods

        #region Protected Instance Methods

        protected TKey? Parse(string? id)
        {
            if (id == null)
            {
                return default;
            }
            var type = typeof(TKey);
            if (type == typeof(ObjectId))
            {
                var objectId = ObjectId.Parse(id);
                return (TKey)(object)objectId;
            }
            if (type == typeof(int))
            {
                var integerId = Convert.ToInt32(id);
                return (TKey)(object)integerId;
            }
            throw new ArgumentException("Invalid Key Type");
        }

        protected bool Equate(TKey left, TKey right)
        {
            if (!(left == null) && !(right == null))
            {
                return left.Equals(right);
            }
            return false;
        }

        #endregion Protected Generic Methods


    }

#nullable restore

}

