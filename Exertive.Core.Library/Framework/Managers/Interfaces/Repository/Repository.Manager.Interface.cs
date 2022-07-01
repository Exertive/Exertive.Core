namespace Exertive.Core.Framework.Managers
{

    #region Dependencies

    using System;

    using Exertive.Core.Domain.Entities;
    using Exertive.Core.Framework.Repositories;

    #endregion Dependencies

    public interface IRepositoryManager<TKey> where TKey : IEquatable<TKey>
    {

        #region Interface Methods

        void Register<TEntity>(IRepository<TEntity, TKey> repository) where TEntity : Entity<TKey>;

        bool Contains<TEntity>();

        IRepository<TEntity, TKey> Access<TEntity>() where TEntity : Entity<TKey>;

        #endregion Interface Methods

    }
}
