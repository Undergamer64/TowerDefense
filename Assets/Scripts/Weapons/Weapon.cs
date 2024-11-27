using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Weapon : MonoBehaviour
{
    public WeaponType Type;

    [SerializeField] protected List<WeaponStat> _upgradeStats;

    [SerializeField] protected GameObject _projectilePrefab;
 
    protected WeaponStat _weaponStat;
    private float _cooldown = 0;

    protected Transform _target;

    protected ComponentPool<Projectile> _projectilePool;
    protected List<Projectile> _projectilesAlive = new List<Projectile>();

    public int Level { get; protected set; } = 0;

    private void Start()
    {
        Level = 1;
        _weaponStat = _upgradeStats[Level - 1];
        _projectilePool = new ComponentPool<Projectile>(_projectilePrefab, 50, 10);
    }

    public void Tick()
    {
        if (_cooldown > 0)
        {
            _cooldown -= Time.deltaTime;
        }

        if (_cooldown <= 0) 
        {
            if (TryShoot())
            {
                Shoot();
                _cooldown = _weaponStat.ReloadTime;
            }
        }
    }
    
    protected void RemoveProjectile(Projectile projectile)
    {
        _projectilesAlive.Remove(projectile);
    }

    public abstract void MoveProjectiles();

    private bool TryShoot()
    {
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, _weaponStat.Range)
            .Where(x => x.GetComponent<Enemy>() is not null).ToList();

        _target = GetNearestTarget(colliders);

        return _target;
    }

    private Transform GetNearestTarget(List<Collider2D> colliders)
    {
        float minDistance = _weaponStat.Range + 1;
        Transform target = null;
        foreach (Collider2D collider in colliders) 
        {
            float distance = Vector3.Distance(collider.transform.position, transform.position);
            if (distance < minDistance) 
            {
                minDistance = distance;
                target = collider.transform;
            }
        }
        return target;
    }

    protected abstract void Shoot();

    protected void SpawnProjectile()
    {
        Projectile projectile = _projectilePool.Get();
        projectile.transform.position = transform.position + (_target.position - transform.position).normalized/10;
        projectile.DespawnCooldown = _weaponStat.BulletLifeTime; 
        projectile.Pierce = _weaponStat.Pierce;
        _projectilesAlive.Add(projectile);
        projectile.OnDestroy.AddListener(RemoveProjectile);
    }
    
    public void OnDrawGizmos()
    {
        if (!_weaponStat) return;
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _weaponStat.Range);
    }

    public virtual bool TryUpgrade()
    {
        if (Level >= _upgradeStats.Count)
        {
            return false;
        }
        
        _weaponStat = _upgradeStats[Level];
        
        Level++;
        
        _cooldown = _weaponStat.ReloadTime;
        
        return true;
    }
}

public enum WeaponType
{
    Riffle,
    Snipper,
    Cannon
}