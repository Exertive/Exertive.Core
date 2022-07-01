
namespace Exertive.Core.Framework.Managers
{

    #region Dependencies

    using System;
    using System.Collections.Generic;

    using Exertive.Core.Domain.Entities;

    using MongoDB.Driver;

    #endregion Dependencies

    public class CollectionManager<TKey> : ICollectionManager<TKey> where TKey : IEquatable<TKey>
    {

        #region Public Instance Propeties

        public IDictionary<Type, object> Collections
        {
            get;
            private set;
        }

        public CollectionManager()
        {
            this.Collections = new Dictionary<Type, object>();
        }

        #endregion Public Instance Propeties

        #region Public Instance Methods

        public void Register<TEntity>(IMongoCollection<TEntity> collection) where TEntity : Entity<TKey>
        {
            if (!this.Contains<TEntity>())
            {
                this.Collections.Add(typeof(TEntity), collection);
            }
        }

        public bool Contains<TEntity>()
        {
            return this.Collections.ContainsKey(typeof(TEntity));
        }

        public IMongoCollection<TEntity> Access<TEntity>() where TEntity : Entity<TKey>
        {
            if (this.Contains<TEntity>())
            {
                var collection = this.Collections[typeof(TEntity)];
                return collection as IMongoCollection<TEntity>;
            }
            throw new InvalidOperationException("");
        }

        #endregion Public Instance Methods

    }

}
