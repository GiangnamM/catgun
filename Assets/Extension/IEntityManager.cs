using System.Collections.Generic;

using JetBrains.Annotations;

using Extension;

using UnityEngine;

namespace App {
    public class EntityObserver {
    }

    public interface IEntityManager : IObserverManager<EntityObserver> {
        /// <summary>
        /// Gets the main view.
        /// </summary>
        [NotNull]
        Transform View { get; }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        [NotNull]
        List<Entity> Entities { get; }
        
        /// <summary>
        /// Adds the specified entity to this manager.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void AddEntity([NotNull] Entity entity);

        /// <summary>
        /// Removes the specified entity from this manager.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void RemoveEntity([NotNull] Entity entity);

        /// <summary>
        /// Finds all entities of the specified type.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <returns>All found entities.</returns>
        [NotNull]
        List<T> FindEntities<T>() where T : Entity;

        [NotNull]
        List<T> FindComponents<T>() where T : EntityComponent;
        
    }
}