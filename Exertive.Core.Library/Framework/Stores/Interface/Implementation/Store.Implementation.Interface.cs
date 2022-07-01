namespace Exertive.Core.Framework.Stores
{

    using System;

    public interface IStoreImplementation<TKey> where TKey : IEquatable<TKey>
    {

        Type Entity { get; }

        Type Definition { get; }

        object Store { get; }

    }

}
