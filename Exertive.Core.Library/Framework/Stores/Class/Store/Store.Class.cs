namespace Exertive.Core.Framework.Stores
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Driver;
#if MAPPED
    using Exertive.Core.Domain.Models;
#endif // MAPPED
    using Exertive.Core.Extensions;
    using Exertive.Core.Domain.Entities;

    public abstract class Store<TKey> : IStore<TKey>
    {

        #region Protected Generic Properties

        protected static IStore<TKey> Instance { get; set; }

        #endregion Protected Generic Properties

        #region Public Generic Methods

#if MAPPED

#if SYNCHRONOUS

        #region Synchronous Methods

        public List<TModel> Get<TEntity, TModel>() where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new()
        {
            return this.Collection<TEntity>().Find((entity) => true).Project((entity) => entity.ToModel<TEntity, TModel>()).ToList();
        }

        public TModel Get<TEntity, TModel>(string id) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new()
        {
            TKey key = this.Parse(id);
            return this.Collection<TEntity>().Find((TEntity instance) => this.Equate(instance.Id, key)).Project((TEntity instance) => instance.ToModel<TEntity, TModel>()).FirstOrDefault();
        }

        public List<TModel> Find<TEntity, TModel>(string key, string value) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new()
        {
            return this.Collection<TEntity>().Find(new BsonDocument(key.ToCamelCase(), value)).Project((TEntity instance) => instance.ToModel<TEntity, TModel>()).ToList<TModel>();
        }

        public List<TModel> Find<TEntity, TModel>(string key, Guid value) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new()
        {
            return this.Collection<TEntity>().Find(new BsonDocument(key.ToCamelCase(), new BsonBinaryData(value, GuidRepresentation.Standard))).Project((TEntity instance) => instance.ToModel<TEntity, TModel>()).ToList<TModel>();
        }

        public List<TModel> Find<TEntity, TModel>(string key, long value) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new()
        {
            var filter = new BsonDocument(key.ToCamelCase(), new BsonDocument("$eq", new BsonInt64(value)));
            return this.Collection<TEntity>().Find(filter).Project((TEntity instance) => instance.ToModel<TEntity, TModel>()).ToList<TModel>();
        }

        public TModel Create<TEntity, TModel>(TModel model) where TEntity : class, IEntity<TKey>, new() where TModel : class, IModel<TKey>, new()
        {
            var entity = model.ToEntity<TEntity, TModel>();
            this.Collection<TEntity>().InsertOne(entity);
            return entity.ToModel<TEntity, TModel>();
        }

        public TModel Update<TEntity, TModel>(TModel model) where TEntity : class, IEntity<TKey>, new() where TModel : class, IModel<TKey>, new()
        {
            var key = this.Parse(model.Id);  
            var entity = model.ToEntity<TEntity, TModel>();
            this.Collection<TEntity>().ReplaceOne((instance) => this.Equate(instance.Id, key), entity);
            return entity.ToModel<TEntity, TModel>();
        }

        public void Remove<TEntity, TModel>(TModel model) where TEntity : IEntity<TKey> where TModel : IModel<TKey>, new()
        {
            var key = this.Parse(model.Id);
            this.Collection<TEntity>().DeleteOne((TEntity instance) => this.Equate(instance.Id, key));
        }

        public List<TModel> Select<TEntity, TModel>(string field, string value) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new()
        {
            var filter = new BsonDocument(field.ToCamelCase(), value);
            return this.Collection<TEntity>().Find(filter).Project((TEntity instance) => (instance.ToModel<TEntity, TModel>())).ToList<TModel>();
        }

        #endregion Synchronous Methods

#endif // SYNCHRONOUS

        #region Asynchronous Methods

        public Task<List<TModel>> GetAsync<TEntity, TModel>() where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new()
        {
            return this.Collection<TEntity>().Find((entity) => true).Project((TEntity instance) => instance.ToModel<TEntity, TModel>()).ToListAsync();
        }

        public Task<List<TModel>> FindAsync<TEntity, TModel>(string key, string value) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new()
        {
            return this.Collection<TEntity>().Find(new BsonDocument(key.ToCamelCase(), value)).Project((TEntity instance) => instance.ToModel<TEntity, TModel>()).ToListAsync<TModel>();
        }

        public Task<List<TModel>> FindAsync<TEntity, TModel>(string key, Guid value) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new()
        {
            return this.Collection<TEntity>().Find(new BsonDocument(key.ToCamelCase(), new BsonBinaryData(value, GuidRepresentation.Standard))).Project((TEntity instance) => instance.ToModel<TEntity, TModel>()).ToListAsync<TModel>();
        }

        public Task<TModel> GetAsync<TEntity, TModel>(string id) where TEntity : class, IEntity<TKey> where TModel : class, IModel<TKey>, new()
        {
            var key = this.Parse(id);
            return this.Collection<TEntity>().Find<TEntity>((TEntity instance) => this.Equate(instance.Id, key)).Project((TEntity instance) => instance.ToModel<TEntity, TModel>()).FirstOrDefaultAsync();
        }

        public Task<TModel> CreateAsync<TEntity, TModel>(TModel model) where TEntity : class, IEntity<TKey>, new() where TModel : class, IModel<TKey>, new()
        {
            var entity = model.ToEntity<TEntity, TModel>();
            this.Collection<TEntity>().InsertOne(entity);
            return new Task<TModel>(() => model);
        }

        public Task<TModel> UpdateAsync<TEntity, TModel>(TModel model) where TEntity : class, IEntity<TKey>, new() where TModel : class, IModel<TKey>, new()
        {
            var key = this.Parse(model.Id);
            this.Collection<TEntity>().ReplaceOne((TEntity instance) => this.Equate(instance.Id, key), model.ToEntity<TEntity, TModel>());
            return new Task<TModel>(() => model);
        }

        public Task<TModel> RemoveAsync<TEntity, TModel>(TModel model) where TEntity : IEntity<TKey> where TModel : IModel<TKey>, new()
        {
            var key = this.Parse(model.Id);
            this.Collection<TEntity>().DeleteOne((TEntity instance) => this.Equate(instance.Id, key));
            return new Task<TModel>(() => model);
        }

        #endregion Asynchronous Methods

#else // UNMAPPED

        #region Asynchronous Methods

        public Task<List<TEntity>> GetAsync<TEntity>() where TEntity : class, IEntity<TKey>
        {
            return this.Collection<TEntity>().FindAsync((entity) => true).Result.ToListAsync();
        }

        public Task<List<TEntity>> FindAsync<TEntity>(string key, string value) where TEntity : class, IEntity<TKey>
        {
            return this.Collection<TEntity>().FindAsync(new BsonDocument(key.ToCamelCase(), value)).Result.ToListAsync();
        }

        public Task<List<TEntity>> FindAsync<TEntity>(string key, Guid value) where TEntity : class, IEntity<TKey>
        {
            return this.Collection<TEntity>().FindAsync(new BsonDocument(key.ToCamelCase(), new BsonBinaryData(value, GuidRepresentation.Standard))).Result.ToListAsync();
        }

        public Task<TEntity> GetAsync<TEntity>(TKey id) where TEntity : class, IEntity<TKey>
        {
            return this.Collection<TEntity>().FindAsync((instance) => this.Equate(instance.Id, id)).Result.FirstOrDefaultAsync();
        }

        public async Task<TEntity> CreateAsync<TEntity>(TEntity instance) where TEntity : class, IEntity<TKey>
        {
            await this.Collection<TEntity>().InsertOneAsync(instance);
            return instance;
        }

        public async Task<TEntity> UpdateAsync<TEntity>(TEntity instance) where TEntity : class, IEntity<TKey>
        {
            var result = await this.Collection<TEntity>().ReplaceOneAsync((entry) => this.Equate(entry.Id, instance.Id), instance);
            return instance;
        }

        public async Task<TEntity> RemoveAsync<TEntity>(TEntity instance) where TEntity : IEntity<TKey>
        {
            var result = await this.Collection<TEntity>().DeleteOneAsync((entry) => this.Equate(entry.Id, instance.Id));
            return instance;
        }

        #endregion Asynchronous Methods


#endif // MAPPED / UNMAPPED

        #endregion Public Generic Methods

        #region Protected Generic Methods

        protected abstract IMongoCollection<TEntity> Collection<TEntity>() where TEntity : IEntity<TKey>;

        protected TKey Parse(string id)
        {
            var type = typeof(TKey);
            if (type == typeof(ObjectId))
            {
                var objectId = ObjectId.Parse(id);
                return (TKey)(object)objectId;
            }
            if (type == typeof(int))
            {
                var integerId = Convert.ToInt32(id);
                return (TKey)(object)integerId;
            }
            throw new ArgumentException("Invalid Key Type");
        }

        protected bool Equate(TKey left, TKey right)
        {
            return left.Equals(right);
        }

        #endregion Protected Generic Methods


    }
}

