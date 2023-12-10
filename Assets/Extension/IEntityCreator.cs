using System;

namespace App {
    public interface IEntityCreator {
        T Create<T>() where T : Entity;
        Entity Create(Type type);
    }
}