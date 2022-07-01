#region Header

// <header>
// Exertive Core Library
// -------------------------------------------------------------------------------
// Entity Repository Interface Definition:
// <summary>
// Represents an Entity Repository which abstracts from the underlying
// Entity Store, and manages the collection of Entity Entries it persists,
// providing the standard range of data management and retrieval operations.
// </summary>
// -------------------------------------------------------------------------------
// =><= © Exertive Technology 2020->
// <license>
// MIT License
//
// Copyright (c) [2020->] [Exertive Technology Ltd.]
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// <license>
// </header>

#endregion Header


namespace Exertive.Core.Framework.Repositories
{
    using System;
    using System.Collections.Generic;
#if LINQ
    using System.Linq;
#endif
    using System.Threading.Tasks;

    using Exertive.Core.Domain.Entities;
    using Exertive.Core.Framework.Managers;

    using MongoDB.Driver;

    /// <summary>
    /// Repressents an Entity Repository which abstracts from the underlying Entity Store, and manage the collection of 
    /// Assignment Entries it persists, providing the standard range of data management and retrieval operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the Entity Entry in the Repository.</typeparam>
    /// <typeparam name="TKey">The type of the Primary Key Identifier of each Assignment.</typeparam>

    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {

        #region Public Instance Properties

        /// <summary>
        /// The underlying MongoDb Collection of Recovery Code Entity instances
        /// </summary>
        public virtual IMongoCollection<TEntity> Collection
        {
            get;
            protected set;
        }

        #endregion Protected Instance Fields

        #region Private Instance Fields

        private bool _disposed;

        #endregion Private Instance Fields

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the Entity Repository, instantiating the underlying MongoDb collection using the
        /// Collection Manager provided.
        /// </summary>
        /// <param name="collectionProvider">The MongoDb Collection Manager which provides the collection of Entity documents.</param>
        protected Repository(ICollectionManager<TKey> collectionManager)
        {
            this.Collection = collectionManager.Access<TEntity>();
        }

        #endregion Constructor

        #region Interface Methods

        public virtual Task<TEntity> FindByIdAsync(TKey identifier)
        {
            return this.Collection.FindAsync((entry) => entry.Id!.Equals(identifier)).Result.SingleOrDefaultAsync();
        }

        public abstract Task<TEntity> FindByNameAsync(string name);

        public virtual Task<List<TEntity>> GetAllAsync()
        {
            return this.Collection.FindAsync((entry) => true).Result.ToListAsync();
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            await this.Collection.InsertOneAsync(entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await this.Collection.ReplaceOneAsync((entry) => entry.Id.Equals(entity.Id), entity);
            return entity;
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
            return this.Collection.DeleteOneAsync((entry) => entry.Id.Equals(entity.Id));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                }
                this._disposed = true;
            }
        }

        #endregion Interface Methods

    }
}
