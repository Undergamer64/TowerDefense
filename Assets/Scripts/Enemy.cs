using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour, IPoolableObject<Enemy>
{
    public Pool<Enemy> Pool { get; set; }

    public UnityEvent<Enemy> OnDestroy;

    public float Speed = 2f;
    public float Life = 2f;

    public void TakeDamage(float damage)
    {
        Life -= damage;
        if (Life <= 0)
        {
            OnDestroy.Invoke(this);
            Pool.Release(this);
        }
    }
}
