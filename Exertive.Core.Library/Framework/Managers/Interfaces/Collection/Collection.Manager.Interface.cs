namespace Exertive.Core.Framework.Managers
{

    #region Dependencies

    using System;

    using MongoDB.Driver;

    using Exertive.Core.Domain.Entities;

    #endregion Dependencies

    public interface ICollectionManager<TKey> where TKey : IEquatable<TKey>
    {

        #region Interface Methods

        void Register<TEntity>(IMongoCollection<TEntity> collection) where TEntity : Entity<TKey>;

        bool Contains<TEntity>();

        IMongoCollection<TEntity> Access<TEntity>() where TEntity : Entity<TKey>;

        #endregion Interface Methods

    }
}
