using UnityEngine;

public class CannonProjectile : Projectile
{
    [SerializeField] private GameObject _radiusDisplay;
    
    public float _explosionRange;
    
    protected override void HitEnemies()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, _collider.radius))
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
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, _explosionRange/2))
        {
            if (!collider.TryGetComponent(out Enemy enemy)) continue;
            enemy.TakeDamage(Damage);
        }
    }

    public void UpdateRadiusDisplay()
    {
        _radiusDisplay.transform.localScale = Vector3.one * (_explosionRange * (1/transform.localScale.x));
        Debug.Log(_radiusDisplay.transform.localScale);
        Debug.Log(_radiusDisplay.transform.lossyScale);
    }
}
