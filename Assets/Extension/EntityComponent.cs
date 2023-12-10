 using JetBrains.Annotations;

using UnityEngine;

namespace App {
    [RequireComponent(typeof(Entity))]
    public class EntityComponent : MonoBehaviour {
        private bool _isEntityCached;
        private Entity _cachedEntity;

        /// <summary>
        /// Gets the associated entity.
        /// </summary>
        [NotNull]
        public Entity Entity {
            get {
                if (_isEntityCached) {
                    return _cachedEntity;
                }
                _isEntityCached = true;
                _cachedEntity = GetComponent<Entity>();
                return _cachedEntity;
            }
        }

        /// <summary>
        /// Gets the associated level manager.
        /// </summary>
        public ILevelManager LevelManager => Entity.LevelManager;
    }
}