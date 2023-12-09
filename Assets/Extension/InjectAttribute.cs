using System;

namespace Rainbow5s {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectAttribute : Attribute {
        public string Tag { get; }

        public InjectAttribute(string tag = "") {
            Tag = tag;
        }
    }
}