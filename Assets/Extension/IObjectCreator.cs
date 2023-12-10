using JetBrains.Annotations;

namespace App {
    public interface IObjectCreator<out T> {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <returns>A new instance.</returns>
        [NotNull]
        T Create();
    }
}