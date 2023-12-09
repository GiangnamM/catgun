using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Rainbow5s {
    public class ObserverManager<T> : IObserverManager<T> {
        [NotNull]
        private readonly Dictionary<int, T> _observers;

        private int _counter;

        public ObserverManager() {
            _observers = new Dictionary<int, T>();
            _counter = 0;
        }

        public int AddObserver(T observer) {
            var id = _counter++;
            _observers.Add(id, observer);
            return id;
        }

        public bool RemoveObserver(int id) {
            return _observers.Remove(id);
        }

        public void DispatchEvent(Action<T> dispatcher) {
            var observers = _observers.ToArray();
            foreach (var entry in observers) {
                dispatcher(entry.Value);
            }
        }
    }
}