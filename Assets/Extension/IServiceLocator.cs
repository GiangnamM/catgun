using JetBrains.Annotations;

namespace Extension {
    public interface IServiceLocator {
        void Provide<T>([NotNull] T service);
        [NotNull]
        T Resolve<T>();
        void ResolveInjection<T>([NotNull] T value, [NotNull] string tag = "");
    }
}