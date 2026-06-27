using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class Pool<T> : IPool<T> where T : IPoolableObject<T>
{
    private Stack<T> _pooledObjects;

    private Func<T> _createFunc;
    private Action<T> _onGetFunc;
    private Action<T> _onReleasedFunc;

    private int _aliveObjectCount = 0;

    public int PooledObjectCount => _pooledObjects.Count;
    public int AliveObjectsCount => _aliveObjectCount;

    public Pool(Func<T> createFunc, int capacity = 50, int preloadedItems = 0) : this(createFunc, null, null, capacity, preloadedItems) { }

    public Pool(Func<T> createFunc, Action<T> onGetFunc, Action<T> onReleasedFunc, int capacity = 50, int preloadedItems = 0)
    {
        Assert.IsNotNull(createFunc);
        Assert.IsTrue(capacity > 0);
        Assert.IsTrue(preloadedItems >= 0);

        _pooledObjects = new Stack<T>(capacity);
        _createFunc = createFunc;
        _onGetFunc = onGetFunc;
        _onReleasedFunc = onReleasedFunc;
        PreloadItems(preloadedItems);
    }

    private void PreloadItems(int preloadedItems)
    {
        for (int i = 0; i < preloadedItems; i++)
        {
            Release(CreateItem());
        }
    }

    private T CreateItem()
    {
        T item = _createFunc.Invoke();
        item.SetPool(this);
        return item;
    }

    public T Get()
    {
        T item;
        _aliveObjectCount++;
        if (_pooledObjects.Count > 0)
        {
            item = _pooledObjects.Pop();
        }
        else
        {
            item = CreateItem();
        }
        if (_createFunc != null)
        {
            _onGetFunc.Invoke(item);
        }
        return item;
    }

    public void Release(T item)
    {
        _pooledObjects.Push(item);
        if (_onReleasedFunc != null)
        {
            _onReleasedFunc.Invoke(item);
        }
        _aliveObjectCount--;
    }
}
