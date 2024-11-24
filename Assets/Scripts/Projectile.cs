using UnityEngine;

public class Projectile : MonoBehaviour, IPoolableObject<Projectile>
{
    private CircleCollider2D _collider;
    private Transform _transform;

    public float DespawnCooldown;
    public int Pierce = 1;
    public float Damage = 1;

    public Weapon Spawner;

    private Pool<Projectile> _pool;
    public Pool<Projectile> Pool { get => _pool; set => _pool = value; }

    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        DespawnCooldown -= Time.deltaTime;
        if (DespawnCooldown < 0 )
        {
            Spawner.ProjectilesAlive.Remove(this);
            Pool.Release(this);
        }

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(_transform.position, _collider.radius))
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(Damage);
                Pierce--;
                if (Pierce <= 0)
                {
                    Pool.Release(this);
                    return;
                }
            }
        }
    }
}
