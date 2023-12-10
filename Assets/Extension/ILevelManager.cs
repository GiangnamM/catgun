using JetBrains.Annotations;

namespace App {
    public interface ILevelManager {
        /// <summary>
        /// Gets the config manager.
        /// </summary>
        [NotNull]
        IConfigManager ConfigManager { get; }

        /// <summary>
        /// Gets the entity manager.
        /// </summary>
        [NotNull]
        IEntityManager EntityManager { get; }


        /// <summary>
        /// Gets the pool manager.
        /// </summary>
        [NotNull]
        IPoolManager PoolManager { get; }
    }
}