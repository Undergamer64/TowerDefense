using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour, IPoolableObject<Projectile>
{
    protected CircleCollider2D _collider;
    protected Transform _transform;

    public UnityEvent<Projectile> OnDestroy;
    
    public float DespawnCooldown;
    public int Pierce = 1;
    public float Damage = 1;

    public Pool<Projectile> Pool { get; set; }

    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        DespawnCooldown -= Time.fixedDeltaTime;
        if (DespawnCooldown < 0 )
        {
            Despawn();
        }

        HitEnemies();
    }

    protected virtual void Despawn()
    {
        OnDestroy.Invoke(this);
        Pool.Release(this);
    }

    protected virtual void HitEnemies()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(_transform.position, _collider.radius))
        {
            if (!collider.TryGetComponent(out Enemy enemy)) continue;
            enemy.TakeDamage(Damage);
            Pierce--;
            if (Pierce > 0) continue;
            Despawn();
            return;
        }
    }
}
