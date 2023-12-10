
using UnityEngine;

namespace App {
    [DisallowMultipleComponent]
    public class Entity : MonoBehaviour {
        /// <summary>
        /// Gets or sets the level manager.
        /// </summary>

        public ILevelManager LevelManager { get; set; }
        
        /// <summary>
        /// Checks whether this entity is alive or not.
        /// </summary>
        public bool IsAlive { get; private set; } = true;

        /// <summary>
        /// Resurrects this entity.
        /// </summary>
        /// <returns>True if the process was successful, false otherwise.</returns>
        public bool Resurrect() {
            if (IsAlive) {
                return false;
            }
            IsAlive = true;
            return true;
        }

        /// <summary>
        /// Kills this entity.
        /// </summary>
        /// <returns>True if the process was successful, false otherwise.</returns>
        public bool Kill() {
            if (!IsAlive) {
                return false;
            }
            IsAlive = false;
            LevelManager.EntityManager.RemoveEntity(this);
            return true;
        }
    }
}