namespace Exertive.Core.Framework.Managers
{

    #region Dependencies

    using System;

    #endregion Dependencies

    public interface IStoreManager<TKey> where TKey : IEquatable<TKey>
    {

        #region Interface Methods

        void Register<TStore>(TStore store) where TStore : class;

        bool Contains(Type type);

        object Access(Type type);

        #endregion Interface Methods

    }
}
