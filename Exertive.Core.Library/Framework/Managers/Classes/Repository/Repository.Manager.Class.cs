namespace Exertive.Core.Framework.Managers
{

    #region Dependencies

    using System;
    using System.Collections.Generic;

    using Exertive.Core.Domain.Entities;
    using Exertive.Core.Framework.Repositories;

    #endregion Dependencies

    public class RepositoryManager<TKey> : IRepositoryManager<TKey> where TKey : IEquatable<TKey>
    {

        #region Protected Instance Properties

        protected Dictionary<Type, object> Repositories
        {
            get;
            private set;
        }

        #endregion Protected Instance Properties

        #region Constructor

        public RepositoryManager()
        {
            this.Repositories = new Dictionary<Type, object>();
        }

        #endregion Constructor

        #region Public Instance Methods

        /// <summary>
        /// Accesses the Repository in the Repository Registry which persists Entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The type of Entity for which to Access the Repository.</typeparam>
        /// <returns>The Repository matching the specified Entity Type.</returns>
        /// <exception cref="InvalidOperationException">Exception throw if a matching Repository is not present in the Repository Registry.</exception>
        public IRepository<TEntity, TKey> Access<TEntity>() where TEntity : Entity<TKey>
        {
            if (this.Contains<TEntity>())
            {
                var repository = this.Repositories[typeof(TEntity)];
                return repository as IRepository<TEntity, TKey>;
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Determines whether the Repository Registry contains a Repository which persists Entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The type of Entity for which to determine the existence in the Repository Registry.</typeparam>
        /// <returns></returns>
        public Boolean Contains<TEntity>()
        {
            return this.Repositories.ContainsKey(typeof(TEntity));
        }

        public void Register<TEntity>(IRepository<TEntity, TKey> repository) where TEntity : Entity<TKey>
        {
            if (!this.Contains<TEntity>())
            {
                this.Repositories.Add(typeof(TEntity), repository);
            }
        }

        #endregion Public Instance Methods

    }

}
