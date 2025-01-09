using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour, IPoolableObject<Enemy>
{
    public Pool<Enemy> _Pool { get; set; }

    [FormerlySerializedAs("OnDestroy")] public UnityEvent<Enemy> _OnDestroy;

    public EnemyType _Type;
    
    [FormerlySerializedAs("Speed")] public float _Speed = 2f;
    [FormerlySerializedAs("Life")] public float _Life = 2f;

    public void TakeDamage(float damage)
    {
        _Life -= damage;
        if (_Life <= 0)
        {
            _OnDestroy.Invoke(this);
            _Pool.Release(this);
        }
    }
}
