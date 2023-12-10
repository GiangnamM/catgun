namespace App {
    public interface IObjectPool<T> : IObjectCreator<T>, IObjectDestroyer<T> {
    }
}