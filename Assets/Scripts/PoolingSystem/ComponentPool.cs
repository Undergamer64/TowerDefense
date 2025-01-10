using UnityEngine;
using UnityEngine.Assertions;

public class ComponentPool<T> : IPool<T> where T : Component, IPoolableObject<T>
{
    private Pool<T> _pool;

    public ComponentPool(GameObject prefab, int capacity = 50, int preloadQuantity = 0, Transform parent = null)
    {
        Assert.IsNotNull(prefab);
        
        _pool = new Pool<T>(
            () => 
            { 
                GameObject gameObject = parent ? Object.Instantiate(prefab, parent) : Object.Instantiate(prefab);
                return gameObject.GetComponent<T>(); 
            },
            (item) => { item.gameObject.SetActive(true); },
            (item) => { item.gameObject.SetActive(false); },
            capacity, 
            preloadQuantity
            );
    }

    public int PooledObjectCount => _pool.PooledObjectCount;

    public int AliveObjectsCount => _pool.AliveObjectsCount;

    public T Get()
    {
        return _pool.Get();
    }

    public void Release(T item)
    {
        _pool.Release(item);
    }
}
