namespace Exertive.Core.Framework.Stores
{

    #region Dependencies

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Exertive.Core.Domain.Entities;
#if MAPPED
    using Exertive.Core.Domain.Models;
#endif // MAPPED

    #endregion Dependencies

    public interface IStore<TKey>
    {

        #region Interface Methods

#if MAPPED

#if SYNCHRONOUS

        #region Synchronous Methods

        List<TModel> Get<TEntity, TModel>() where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new();

        List<TModel> Find<TEntity, TModel>(string key, string value) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new();

        List<TModel> Find<TEntity, TModel>(string key, Guid value) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new();

        List<TModel> Find<TEntity, TModel>(string key, long value) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new();

        TModel Get<TEntity, TModel>(string identifier) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new();

        TModel Create<TEntity, TModel>(TModel model) where TEntity : class, IEntity<TKey>, new() where TModel : class, IModel<TKey>, new();

        TModel Update<TEntity, TModel>(TModel model) where TEntity : class, IEntity<TKey>, new() where TModel : class, IModel<TKey>, new();

        void Remove<TEntity, TModel>(TModel model) where TEntity : IEntity<TKey> where TModel : IModel<TKey>, new();

        #endregion Synchronous Methods

#endif // SYNCHRONOUS

        #region Asynchronous Methods

        Task<List<TModel>> GetAsync<TEntity, TModel>() where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new();

        Task<List<TModel>> FindAsync<TEntity, TModel>(string key, string value) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new();

        Task<List<TModel>> FindAsync<TEntity, TModel>(string key, Guid value) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new();

        Task<TModel> GetAsync<TEntity, TModel>(string identifier) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new();

        Task<TModel> CreateAsync<TEntity, TModel>(TModel model) where TEntity : class, IEntity<TKey>, new() where TModel : class, IModel<TKey>, new();

        Task<TModel> UpdateAsync<TEntity, TModel>(TModel model) where TEntity : class, IEntity<TKey>, new() where TModel : class, IModel<TKey>, new();

        Task<TModel> RemoveAsync<TEntity, TModel>(TModel model) where TEntity : IEntity<TKey> where TModel : IModel<TKey>, new();

        #endregion Asynchronous Methods

#else // UNMAPPED

        #region Asynchronous Methods

        Task<List<TEntity>> GetAsync<TEntity>() where TEntity : class, IEntity<TKey>;

        Task<List<TEntity>> FindAsync<TEntity>(string key, string value) where TEntity : class, IEntity<TKey>;

        Task<List<TEntity>> FindAsync<TEntity>(string key, Guid value) where TEntity : class, IEntity<TKey>;

        Task<TEntity> GetAsync<TEntity>(TKey identifier) where TEntity : class, IEntity<TKey>;

        Task<TEntity> CreateAsync<TEntity>(TEntity instance) where TEntity : class, IEntity<TKey>;

        Task<TEntity> UpdateAsync<TEntity>(TEntity instance) where TEntity : class, IEntity<TKey>;

        Task<TEntity> RemoveAsync<TEntity>(TEntity instance) where TEntity : IEntity<TKey>;

        #endregion Asynchronous Methods

#endif // MAPPED/UNMAPPED

        #endregion Interface Methods

    }
}

