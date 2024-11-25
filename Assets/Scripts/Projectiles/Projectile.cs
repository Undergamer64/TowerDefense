using UnityEngine;

public class Projectile : MonoBehaviour, IPoolableObject<Projectile>
{
    protected CircleCollider2D _collider;
    protected Transform _transform;

    public float DespawnCooldown;
    public int Pierce = 1;
    public float Damage = 1;

    public Weapon Spawner;

    public Pool<Projectile> Pool { get; set; }

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

        HitEnemies();
    }

    protected virtual void HitEnemies()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(_transform.position, _collider.radius))
        {
            if (!collider.TryGetComponent(out Enemy enemy)) continue;
            enemy.TakeDamage(Damage);
            Pierce--;
            if (Pierce > 0) continue;
            Spawner.ProjectilesAlive.Remove(this);
            Pool.Release(this);
            return;
        }
    }
}
