namespace Exertive.Core.Domain.Entities
{

    #region Dependencies

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Exertive.Core.Domain.Models;

    #endregion Dependencies

    public class EntityCollection<TEntity, TKey> : IReadOnlyCollection<TEntity> where TEntity : class, IEntity<TKey>, new() where TKey : IEquatable<TKey>
    {

        #region Public Instance Properties

        /// <summary>
        /// Get the Type of the Members in the Collection.
        /// </summary>
        public Type Type
        {
            get { return typeof(TEntity); }
        }

        /// <summary>
        /// Get the Count of the number of Members in the Collection.
        /// </summary>
        public int Count
        {
            get { return this._members.Length; }
        }

        #endregion Public Instance Properties

        #region Private Instance Properties

        private TEntity[] _members;

        #endregion Private Instance Properties

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the Collection comprising the Items in the List provided.
        /// </summary>
        /// <param name="items">The List of Items to add as Members of the Collection.</param>
        public EntityCollection(IList<TEntity> items)
        {
            this._members = items.ToArray();
        }

        /// <summary>
        /// Constructs a new instance of the Collection comprising the Items in the Enumerable collection provided.
        /// </summary>
        /// <param name="items">The Enumerable collection of Items to add as Members of the Collection.</param>
        public EntityCollection(IEnumerable<TEntity> items)
        {
            this._members = items.ToArray();
        }

        public bool Contains(TEntity item)
        {
            return !(this._members.FirstOrDefault((member) => member.Id.Equals(item.Id)) == null);
        }

        #endregion Constructor

        #region Public Instance Methods

        #region ICollection Implementation

        /// <summary>
        /// Copys the Members of the Collection to the array provided starting at the specified index position.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The staring index position in the array.</param>
        public void CopyTo(TEntity[] array, int index)
        {
            this._members.CopyTo(array, index);
        }

        /// <summary>
        /// Gets a typed Enumerator for the Members in the Collection.
        /// </summary>
        /// <returns>A typed Enumerator instance.</returns>
        public IEnumerator<TEntity> GetEnumerator()
        {
            return this._members.GetEnumerator() as IEnumerator<TEntity>;
        }

        /// <summary>
        /// Gets a untyped Enumerator for the Members in the Collection.
        /// </summary>
        /// <returns>A untyped Enumerator instance.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._members.GetEnumerator();
        }

        #endregion ICollection Implementation

        /// <summary>
        /// Maps the Collection from an equivalent Model Collection of the specified Model Type.
        /// </summary>
        /// <typeparam name="TModel">The typeof of the Model Collection to map to.</typeparam>
        /// <param name="modelCollection"></param>
        /// <returns></returns>
        public IEntityCollection<TEntity, TKey> FromModelCollection<TModel>(IModelCollection<TModel, TKey> modelCollection) where TModel : class, IModel<TKey>, new()
        {
            this._members = modelCollection.Select((member) => member.ToEntity<TEntity, TModel>()).ToArray();
            return this as IEntityCollection<TEntity, TKey>;
        }

        /// <summary>
        /// Maps the Collection to an equivalent Model Collection of the specified Model Type.
        /// </summary>
        /// <typeparam name="TModel">The typeof of the Model Collection to map to.</typeparam>
        /// <returns></returns>
        public IModelCollection<TModel, TKey> ToModelCollection<TModel>() where TModel : class, IModel<TKey>, new()
        {
            return new ModelCollection<TModel, TKey>(this._members.Select((member) => member.ToModel<TEntity, TModel>())) as IModelCollection<TModel, TKey>;
        }

        #endregion Public Instance Methods
    }
}
