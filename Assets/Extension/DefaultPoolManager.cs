using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace App {
    public class DefaultPoolManager : IPoolManager {
        private class Creator<T> : IObjectCreator<T> where T : Entity {
            [NotNull]
            private readonly IEntityCreator _creator;

            public Creator([NotNull] IEntityCreator creator) {
                _creator = creator;
            }

            public T Create() {
                return _creator.Create<T>();
            }
        }

        [NotNull]
        private readonly IEntityCreator _creator;

        private readonly IDictionary<Type, IObjectPool<Entity>> _pools;

        public DefaultPoolManager([NotNull] IEntityCreator creator) {
            _creator = creator;
            _pools = new Dictionary<Type, IObjectPool<Entity>>();
        }

        public T Create<T>() where T : Entity {
            var instance = Create(typeof(T));
            return (T) instance;
        }

        public Entity Create(Type type) {
            if (!_pools.TryGetValue(type, out var pool)) {
                var creator = Activator.CreateInstance(typeof(Creator<>).MakeGenericType(type), _creator);
                pool = new ComponentPool<Entity>((IObjectCreator<Entity>) creator, null);
                _pools.Add(type, pool);
            }
            var instance = pool.Create();
            instance.Resurrect();
            return instance;
        }

        public bool Destroy(Entity instance) {
            var key = instance.GetType();
            return _pools.TryGetValue(key, out var pool) && pool.Destroy(instance);
        }
    }
}