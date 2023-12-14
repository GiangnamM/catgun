using System;

using JetBrains.Annotations;

namespace Extension {
    [AttributeUsage(AttributeTargets.Interface)]
    public class ServiceAttribute : Attribute {
        [NotNull]
        public string Name { get; }

        public ServiceAttribute([NotNull] string name) {
            Name = name;
        }

        public ServiceAttribute([NotNull] Type type) {
            Name = type.FullName ?? throw new ArgumentNullException(nameof(type), "Missing full name");
        }
    }
}