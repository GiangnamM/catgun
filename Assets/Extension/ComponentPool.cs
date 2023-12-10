using System.Collections.Generic;

using JetBrains.Annotations;


using UnityEngine;
using UnityEngine.Assertions;

namespace App {
    internal class ComponentPoolIndexer : MonoBehaviour {
        public Component Instance { get; set; }
        public int Index { get; set; } = -1;
    }

    public class ComponentPool<T> : IObjectPool<T> where T : Component {
        private class DefaultCreator : IObjectCreator<T> {
            [NotNull]
            private readonly T _prefab;

            public DefaultCreator([NotNull] T prefab) {
                _prefab = prefab;
            }

            public T Create() {
                var instance = Object.Instantiate(_prefab);
                instance.gameObject.SetActive(false);
                return instance;
            }
        }

        private class DefaultDestroyer : IObjectDestroyer<T> {
            public bool Destroy(T instance) {
                instance.gameObject.SetActive(false);
                return true;
            }
        }

        [NotNull]
        private readonly IObjectCreator<T> _creator;

        [NotNull]
        private readonly IObjectDestroyer<T> _destroyer;

        private readonly List<ComponentPoolIndexer> _activeInstances = new List<ComponentPoolIndexer>();
        private readonly List<ComponentPoolIndexer> _inactiveInstances = new List<ComponentPoolIndexer>();

        public ComponentPool(
            [NotNull] IObjectCreator<T> creator,
            [CanBeNull] IObjectDestroyer<T> destroyer) {
            _creator = creator;
            _destroyer = destroyer ?? new DefaultDestroyer();
        }

        public ComponentPool([NotNull] T prefab) {
            _creator = new DefaultCreator(prefab);
            _destroyer = new DefaultDestroyer();
        }

        public ComponentPool([NotNull] string path) : this(Resources.Load<T>(path)) {
        }

        public T Create() {
            var indexer = InstantiateIndexer();
            var instance = indexer.Instance;
            instance.gameObject.SetActive(true);
            return (T) instance;
        }

        public bool Destroy(T instance) {
            var indexer = instance.GetComponent<ComponentPoolIndexer>();
            if (indexer == null) {
                return false;
            }
            DestroyIndexer(indexer);
            return _destroyer.Destroy(instance);
        }

        [NotNull]
        private ComponentPoolIndexer InstantiateIndexer() {
            if (_inactiveInstances.Count == 0) {
                _inactiveInstances.Add(Clone());
            }

            var size = _inactiveInstances.Count;
            var indexer = _inactiveInstances[size - 1];
            _inactiveInstances.RemoveAt(size - 1);

            Assert.IsTrue(indexer.Index == -1);
            indexer.Index = _activeInstances.Count;
            _activeInstances.Add(indexer);

            return indexer;
        }

        private void DestroyIndexer([NotNull] ComponentPoolIndexer indexer) {
            var index = indexer.Index;

            Assert.IsTrue(0 <= index && index < _activeInstances.Count);
            Assert.IsFalse(_inactiveInstances.Contains(indexer));
            _inactiveInstances.Add(indexer);

            indexer.Index = -1;
            var size = _activeInstances.Count;
            if (index < size - 1) {
                _activeInstances[index] = _activeInstances[size - 1];
                _activeInstances[index].Index = index;
            }
            _activeInstances.RemoveAt(size - 1);
        }

        [NotNull]
        private ComponentPoolIndexer Clone() {
            var instance = _creator.Create();
            var indexer = instance.gameObject.AddComponent<ComponentPoolIndexer>();
            indexer.Instance = instance;
            return indexer;
        }
    }
}