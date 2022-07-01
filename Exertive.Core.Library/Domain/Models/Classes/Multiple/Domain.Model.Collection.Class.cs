namespace Exertive.Core.Domain.Models
{

    #region Dependencies

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Exertive.Core.Domain.Entities;

    #endregion Dependencies

    public class ModelCollection<TModel, TKey> : IReadOnlyCollection<TModel> where TModel : class, IModel<TKey>, new() where TKey : IEquatable<TKey>
    {

        #region Public Instance Properties

        /// <summary>
        /// Get the Type of the Members in the Collection.
        /// </summary>
        public Type Type
        {
            get { return typeof(TModel); }
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

        private TModel[] _members;

        #endregion Private Instance Properties

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the Collection comprising the Items in the List provided.
        /// </summary>
        /// <param name="items">The List of Items to add as Members of the Collection.</param>
        public ModelCollection(IList<TModel> items)
        {
            this._members = items.ToArray();
        }

        /// <summary>
        /// Constructs a new instance of the Collection comprising the Items in the Enumerable collection provided.
        /// </summary>
        /// <param name="items">The Enumerable collection of Items to add as Members of the Collection.</param>
        public ModelCollection(IEnumerable<TModel> items)
        {
            this._members = items.ToArray();
        }

        public bool Contains(TModel item)
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
        public void CopyTo(TModel[] array, int index)
        {
            this._members.CopyTo(array, index);
        }

        /// <summary>
        /// Gets a typed Enumerator for the Members in the Collection.
        /// </summary>
        /// <returns>A typed Enumerator instance.</returns>
        public IEnumerator<TModel> GetEnumerator()
        {
            return this._members.GetEnumerator() as IEnumerator<TModel>;
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
        /// Maps the Collection from an equivalent Entity Collection of the specified Entity Type.
        /// </summary>
        /// <typeparam name="TEntity">The typeof of the Entity Collection to map to.</typeparam>
        /// <param name="modelCollection"></param>
        /// <returns></returns>
        public IModelCollection<TModel, TKey> FromEntityCollection<TEntity>(IEntityCollection<TEntity, TKey> entityCollection) where TEntity : class, IEntity<TKey>, new()
        {
            this._members = entityCollection.Select((member) => member.ToModel<TEntity, TModel>()).ToArray();
            return this as IModelCollection<TModel, TKey>;
        }

        /// <summary>
        /// Maps the Collection to an equivalent Entity Collection of the specified Entity Type.
        /// </summary>
        /// <typeparam name="TEntity">The typeof of the Entity Collection to map to.</typeparam>
        /// <returns></returns>
        public IEntityCollection<TEntity, TKey> ToEntityCollection<TEntity>() where TEntity : class, IEntity<TKey>, new()
        {
            return new EntityCollection<TEntity, TKey>(this._members.Select((member) => member.ToEntity<TEntity, TModel>())) as IEntityCollection<TEntity, TKey>;
        }

        #endregion Public Instance Methods

    }
}
