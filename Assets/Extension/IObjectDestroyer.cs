using JetBrains.Annotations;

namespace App {
    public interface IObjectDestroyer<in T> {
        /// <summary>
        /// Destroys the specified instance.
        /// </summary>
        /// <param name="instance">The desired instance to be destroyed.</param>
        bool Destroy([NotNull] T instance);
    }
}