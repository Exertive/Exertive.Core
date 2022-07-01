namespace Exertive.Core.Framework.Managers
{

    using System;
    using System.Collections.Generic;

    public class StoreManager<TKey> : IStoreManager<TKey> where TKey : IEquatable<TKey>
    {

        protected Dictionary<Type, object> Stores
        {
            get;
            private set;
        }

        public StoreManager()
        {
            this.Stores = new Dictionary<Type, object>();
        }

        public object Access(Type type)
        {
            if (this.Contains(type))
            {
                return this.Stores[type];
            }
            throw new InvalidOperationException();
        }

        public bool Contains(Type type)
        {
            return this.Stores.ContainsKey(type);
        }

        public void Register<TStore>(TStore store) where TStore : class
        {
            if (!this.Contains(typeof(TStore)))
            {
                this.Stores.Add(typeof(TStore), store);
            }
        }
    }
}
