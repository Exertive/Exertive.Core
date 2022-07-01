
namespace Exertive.Core.Domain.Entities
{

    #region Dependencies

    using System;
    using System.Collections.Generic;

    using Exertive.Core.Domain.Models;

    #endregion Dependencies

    public interface IEntityCollection<TEntity, TKey> : IReadOnlyCollection<TEntity> where TEntity : class, IEntity<TKey>, new() where TKey : IEquatable<TKey>
    {

        #region Interface Properties

        Type Type { get; }

        #endregion Interface Properties

        #region Interface Methods

        IEntityCollection<TEntity, TKey> FromModelCollection<TModel>(IModelCollection<TModel, TKey> modelCollection) where TModel : class, IModel<TKey>, new();

        IModelCollection<TModel, TKey> ToModelCollection<TModel>() where TModel : class, IModel<TKey>, new();

        #endregion Interface Methods

    }

}
