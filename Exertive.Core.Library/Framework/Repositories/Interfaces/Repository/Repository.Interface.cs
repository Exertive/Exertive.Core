
#region Header

// <header>
// Exertive Core Library
// -------------------------------------------------------------------------------
// Entity Repository Interface Definition:
// <summary>
// Defines an Entity Repository intended to abstract from the underlying
// Entity Store, and manage the collection of Entity Entries it persists,
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
    using System.Threading.Tasks;

    using Exertive.Core.Domain.Entities;

    using MongoDB.Driver;

    /// <summary>
    /// Defines an Entity Repository intended to abstract from the underlying Entity Store, and manage the collection of 
    /// Assignment Entries it persists, providing the standard range of data management and retrieval operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the Entity Entry in the Repository.</typeparam>
    /// <typeparam name="TKey">The type of the Primary Key Identifier of each Assignment.</typeparam>

    public interface IRepository<TEntity, TKey> : IDisposable where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {

        #region Interface Properties

        IMongoCollection<TEntity> Collection { get; }

        #endregion Interface Properties

        #region Interface Methods

        Task<TEntity> FindByIdAsync(TKey identifier);

        Task<TEntity> FindByNameAsync(string name);

        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        #endregion Interface Methods

    }
}
