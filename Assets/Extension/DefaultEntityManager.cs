using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using Extension;

using UnityEngine;
using UnityEngine.Assertions;

namespace App {
    public class DefaultEntityManager : ObserverManager<EntityObserver>, IEntityManager {
        private class IndexCache : MonoBehaviour {
            public class IndexTree {
                private readonly int[] _indices = {-1, -1, -1};

                public int this[int i] {
                    get => _indices[i];
                    set => _indices[i] = value;
                }
            }

            private readonly IDictionary<string, IndexTree> _trees;

            [NotNull]
            public IndexTree EntityTree { get; } = new();

            public IndexCache() {
                _trees = new Dictionary<string, IndexTree>();
            }

            [NotNull]
            public IndexTree GetTree([NotNull] string key) {
                if (_trees.TryGetValue(key, out var result)) {
                    return result;
                }
                var tree = new IndexTree();
                _trees.Add(key, tree);
                return tree;
            }
        }

        private readonly ILevelManager _levelManager;
        private readonly TypeNameCache _nameCache;
        private readonly TypeTreeCache<Entity> _treeCache;

        private readonly IDictionary<string, IList> _cachedComponents;
        
        private int _entityLocker;
        private readonly Queue<Entity> _toBeRemovedEntities;

        private float _accumulatedTime;

        private readonly bool _usingCache;

        public Transform View { get; }

        public List<Entity> Entities { get; }

        public DefaultEntityManager([NotNull] Transform view, [NotNull] ILevelManager levelManager) {
            _levelManager = levelManager;
            _nameCache = new TypeNameCache();
            _treeCache = new TypeTreeCache<Entity>();
            _cachedComponents = new Dictionary<string, IList>();
            _toBeRemovedEntities = new Queue<Entity>();

            _entityLocker = 0;
            _accumulatedTime = 0;
            View = view;
            Entities = new List<Entity>();
        }

        public void AddEntity(Entity entity) {
            Assert.IsTrue(entity.IsAlive);
            AddEntityInstantly(entity);
            entity.gameObject.SetActive(true);
        }

        public void RemoveEntity(Entity entity) {
            _toBeRemovedEntities.Enqueue(entity);
            ProcessEntities();
        }

        private bool AddEntityInstantly([NotNull] Entity entity) {
            // Check existence.
            var indexCache = GetIndexCache(entity);
            var index = indexCache.EntityTree[0];
            if (index != -1) {
                Assert.IsTrue(false);
                return false;
            }

            // Assign entity manager.
            Assert.IsTrue(entity.LevelManager == null);
            entity.LevelManager = _levelManager;

            // Add to cache.
            AddEntityToCache(entity, indexCache);

            // Initialize components.
            var updater = entity.GetComponent<Updater>();
            if (updater != null) {
                updater.Begin();
            }

            // Update index.
            indexCache.EntityTree[0] = Entities.Count;
            Entities.Add(entity);
            return true;
        }

        private void AddEntityToCache(
            [NotNull] Entity entity,
            [NotNull] IndexCache indexCache) {
            AddEntityComponentToCache<Updater>(entity, indexCache);

            var tree = _treeCache.GetTree(entity.GetType());
            var treeSize = tree.Count;
            for (var i = 0; i < treeSize; ++i) {
                var type = tree[i];
                var components = GetCachedComponent(type);
                var indexTree = indexCache.EntityTree;

                // Testing.
                var index = indexTree[i + 1];
                Assert.IsTrue(index == -1);

                // Update index.
                indexTree[i + 1] = components.Count;
                components.Add(entity);
            }
        }

        private void AddEntityComponentToCache<T>(
            [NotNull] Entity entity,
            [NotNull] IndexCache indexCache) where T : EntityComponent {
            var component = entity.GetComponent<T>();
            if (component == null) {
                return;
            }

            var components = GetCachedComponent<T>();
            var key = _nameCache.GetName<T>();
            var indexTree = indexCache.GetTree(key);

            // Testing.
            var index = indexTree[0];
            Assert.IsTrue(index == -1);

            // Update index.
            indexTree[0] = components.Count;
            components.Add(component);
        }

        private bool RemoveEntityInstantly([NotNull] Entity entity) {
            var indexCache = GetIndexCache(entity);
            var index = indexCache.EntityTree[0];
            if (Entities[index] != entity) {
                Assert.IsTrue(false);
                return false;
            }

            // Update index.
            indexCache.EntityTree[0] = -1;
            var size = Entities.Count;
            if (index < size - 1) {
                Entities[index] = Entities[size - 1];
                Entities[index].GetComponent<IndexCache>().EntityTree[0] = index;
            }
            Entities.RemoveAt(size - 1);

            // Un-initialize components.
            var updater = entity.GetComponent<Updater>();
            if (updater != null) {
                updater.End();
            }

            // Remove from cache.
            RemoveEntityFromCache(entity, indexCache);

            // Un-assign entity manager.
            Assert.IsTrue(entity.LevelManager == _levelManager);
            entity.LevelManager = null;
            return true;
        }

        private void RemoveEntityFromCache(
            [NotNull] Entity entity,
            [NotNull] IndexCache indexCache) {
            RemoveEntityComponentFromCache<Updater>(entity, indexCache);

            var tree = _treeCache.GetTree(entity.GetType());
            var treeSize = tree.Count;
            for (var i = 0; i < treeSize; ++i) {
                var type = tree[i];
                var components = GetCachedComponent(type);
                var indexTree = indexCache.EntityTree;

                // Testing.
                var index = indexTree[i + 1];
                Assert.IsTrue((Entity) components[index] == entity);

                // Update index.
                indexTree[i + 1] = -1;
                var size = components.Count;
                if (index < size - 1) {
                    components[index] = components[size - 1];
                    ((Entity) components[index]).GetComponent<IndexCache>().EntityTree[i + 1] = index;
                }
                components.RemoveAt(size - 1);
            }
        }

        private void RemoveEntityComponentFromCache<T>(
            [NotNull] Entity entity,
            [NotNull] IndexCache indexCache) where T : EntityComponent {
            var component = entity.GetComponent<T>();
            if (component == null) {
                return;
            }
            var components = GetCachedComponent<T>();
            var key = _nameCache.GetName<T>();
            var indexTree = indexCache.GetTree(key);

            // Testing.
            var index = indexTree[0];
            Assert.IsTrue(components[index] == component);

            // Update index.
            indexTree[0] = -1;
            var size = components.Count;
            if (index < size - 1) {
                components[index] = components[size - 1];
                components[index].GetComponent<IndexCache>().GetTree(key)[0] = index;
            }
            components.RemoveAt(size - 1);
        }

        [NotNull]
        private IndexCache GetIndexCache([NotNull] Entity entity) {
            var indexCache = entity.GetComponent<IndexCache>();
            if (indexCache == null) {
                indexCache = entity.gameObject.AddComponent<IndexCache>();
            }
            return indexCache;
        }

        [NotNull]
        private List<T> GetCachedComponent<T>() {
            return (List<T>) GetCachedComponent(typeof(T));
        }

        [NotNull]
        private IList GetCachedComponent(Type type) {
            var key = _nameCache.GetName(type);
            if (_cachedComponents.TryGetValue(key, out var result)) {
                return result;
            }
            var components = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
            _cachedComponents.Add(key, components);
            return components;
        }

        private void ProcessEntities() {
            while (true) {
                if (_toBeRemovedEntities.Count == 0) {
                    return;
                }
                if (_entityLocker > 0) {
                    return;
                }
                var entity = _toBeRemovedEntities.Dequeue();
                ++_entityLocker;
                Assert.IsFalse(entity.IsAlive);
                var managed = _levelManager.PoolManager.Destroy(entity);
                RemoveEntityInstantly(entity);
                entity.gameObject.SetActive(false);
                if (!managed) {
                    // Also destroy children.
                    // UnityEngine.Object.Destroy(entity.gameObject);
                }
                --_entityLocker;
            }
        }

        public List<T> FindEntities<T>() where T : Entity {
            return GetCachedComponent<T>();
        }

        public List<T> FindComponents<T>() where T : EntityComponent {
            return GetCachedComponent<T>();
        }
        
    }
}