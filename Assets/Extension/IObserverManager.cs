using System;

using JetBrains.Annotations;

namespace Extension {
    public interface IObserverManager<T> {
        int AddObserver([NotNull] T observer);
        bool RemoveObserver(int id);
        void DispatchEvent([NotNull] Action<T> dispatcher);
    }
}