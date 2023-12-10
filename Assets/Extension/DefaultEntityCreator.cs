using System;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.Assertions;

namespace App {
    [CreateAssetMenu(fileName = "EntityCreator", menuName = "Entity Creator", order = 1)]
    public class DefaultEntityCreator : ScriptableObject, IEntityCreator {
        [SerializeField]
        private List<Entity> _entityPrefabs;

        private readonly IDictionary<Type, Entity> _cachedPrefabs = new Dictionary<Type, Entity>();

        public T Create<T>() where T : Entity {
            return Create(typeof(T)) as T;
        }

        public Entity Create(Type type) {
            var prefab = GetPrefab(type);
            var result = Instantiate(prefab);
            Assert.IsNotNull(result);
            return result;
        }

        private Entity GetPrefab(Type type) {
            if (_cachedPrefabs.TryGetValue(type, out var result)) {
                return result;
            }

            foreach (var prefab in _entityPrefabs) {
                if (prefab.GetComponent(type) != null) {
                    _cachedPrefabs[type] = prefab;
                    return prefab;
                }
            }
            return null;
        }
    }
}