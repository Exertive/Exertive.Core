
namespace Exertive.Core.Domain.Entities
{

    #region Dependencies

    using Exertive.Core.Domain.Models;

    #endregion Dependencies

#nullable enable

    public interface IEntity<TKey>
    {

        #region Interface Properties

        TKey? Id { get; }

        string? Key { get; }

        string? Type { get; }

        #endregion Interface Properties

        #region Interface Methods

        IEntity<TKey> Identify(string? id);

        IEntity<TKey> Identify(TKey? id);

        TEntity Clone<TEntity>() where TEntity : class, IEntity<TKey>, new();

        TEntity FromModel<TEntity, TModel>(TModel model) where TEntity : class, IEntity<TKey>, new() where TModel : IModel<TKey>;

        TModel ToModel<TEntity, TModel>() where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new();

        object? GetValue(string key);

        #endregion Interface Methods

    }

#nullable restore

}
