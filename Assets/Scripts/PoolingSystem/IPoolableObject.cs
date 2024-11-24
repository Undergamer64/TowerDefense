using UnityEngine;

public interface IPoolableObject<T> where T : IPoolableObject<T>
{
    public Pool<T> Pool { get; set; }

    public void SetPool(Pool<T> pool)
    {
        Pool = pool;
    }
}
