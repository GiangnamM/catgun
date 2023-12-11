using System;
using Extension;
using JetBrains.Annotations;

namespace App {
    public interface IPoolManager {
        /// <summary>
        /// Instantiates the specified prefab.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <returns></returns>
        [NotNull]
        T Create<T>() where T : Entity;

        [NotNull]
        Entity Create(Type type);

        /// <summary>
        /// Destroys the specified entity instance.
        /// </summary>
        /// <param name="instance">Entity instance.</param>
        bool Destroy([NotNull] Entity instance);
    }
}