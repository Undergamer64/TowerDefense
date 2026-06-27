public interface IPool<T>
{
    int PooledObjectCount { get; }
    int AliveObjectsCount { get; }

    T Get();
    void Release(T item);
}
