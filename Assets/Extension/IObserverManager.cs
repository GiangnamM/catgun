using System;

using JetBrains.Annotations;

namespace Rainbow5s {
    public interface IObserverManager<T> {
        /// <summary>
        /// Adds an observer.
        /// </summary>
        /// <param name="observer">The observer to be added.</param>
        /// <returns>Observer ID</returns>
        int AddObserver([NotNull] T observer);

        /// <summary>
        /// Removes an observer.
        /// </summary>
        /// <param name="id">The ID of the observer.</param>
        /// <returns>Whether the observer was removed.</returns>
        bool RemoveObserver(int id);

        /// <summary>
        /// Dispatches an event.
        /// </summary>
        /// <param name="dispatcher">The desired dispatcher.</param>
        void DispatchEvent([NotNull] Action<T> dispatcher);
    }
}