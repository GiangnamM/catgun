using JetBrains.Annotations;

namespace Extension {
    public interface IServiceLocator {
        /// <summary>
        /// Registers a service.
        /// </summary>
        void Provide<T>([NotNull] T service);

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        [NotNull]
        T Resolve<T>();

        /// <summary>
        /// Resolves all services for the specified object.
        /// </summary>
        void ResolveInjection<T>([NotNull] T value, [NotNull] string tag = "");
    }
}