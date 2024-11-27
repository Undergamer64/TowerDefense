using UnityEngine;

public class CannonProjectile : Projectile
{
    protected override void HitEnemies()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(_transform.position, _collider.radius))
        {
            if (!collider.TryGetComponent(out Enemy enemy)) continue;
            Despawn();
            return;
        }
    }

    protected override void Despawn()
    {
        Explode();
        base.Despawn();
    }

    private void Explode()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(_transform.position, _collider.radius * 2))
        {
            if (!collider.TryGetComponent(out Enemy enemy)) continue;
            enemy.TakeDamage(Damage);
        }
    }
}
