using UnityEngine;

public class Enemy : MonoBehaviour, IPoolableObject<Enemy>
{
    private Pool<Enemy> _pool;
    public Pool<Enemy> Pool { get => _pool; set => _pool = value; }

    public EnemiesSpawner Spawner;

    public float speed = 1f;
    public float life = 1;

    public void TakeDamage(float damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Spawner.EnemiesAlive.Remove(this);
            Pool.Release(this);
        }
    }
}
