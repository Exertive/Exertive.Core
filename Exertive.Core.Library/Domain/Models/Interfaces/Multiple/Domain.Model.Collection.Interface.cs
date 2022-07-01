
namespace Exertive.Core.Domain.Models
{

    #region Usage

    using System;
    using System.Collections.Generic;

    using Exertive.Core.Domain.Entities;

    #endregion Usage

    public interface IModelCollection<TModel, TKey> : IReadOnlyCollection<TModel> where TModel : class, IModel<TKey>, new() where TKey : IEquatable<TKey>
    {

        #region Interface Properties

        Type Type { get; }

        #endregion Interface Properties

        #region Interface Methods

        IModelCollection<TModel, TKey> FromEntityCollection<TEntity>(IEntityCollection<TEntity, TKey> entityCollection) where TEntity : class, IEntity<TKey>, new();

        IEntityCollection<TEntity, TKey> ToEntityCollection<TEntity>() where TEntity : class, IEntity<TKey>, new();

        #endregion Interface Methods

    }

}
