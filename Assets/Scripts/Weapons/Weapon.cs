using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType Type;

    [SerializeField] protected List<WeaponStat> _upgradeStats;

    [SerializeField] protected GameObject _projectilePrefab;
 
    protected WeaponStat _weaponStat;
    private float _cooldown = 0;

    protected Transform _target;

    [SerializeField] protected int _poolCapacity = 50;
    [SerializeField] protected int _poolPreloadQuantity = 5;
    
    protected ComponentPool<Projectile> _projectilePool;
    protected List<Projectile> _projectilesAlive = new List<Projectile>();
    
    private List<Collider2D> _colliders = new List<Collider2D>();
    
    public int Level { get; private set; } = 0;

    private void Start()
    {
        _projectilePool = new ComponentPool<Projectile>(_projectilePrefab, _poolCapacity, _poolPreloadQuantity);
    }

    private void OnDisable()
    {
        for (int i = 0; i < _projectilesAlive.Count;)
        {
            RemoveProjectile(_projectilesAlive[i]);
        }
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

    public int GetPrice()
    {
        return _upgradeStats[Level].Price;
    }

    public int GetMaxUpgradeLevel()
    {
        return _upgradeStats.Count;
    }
    
    protected void RemoveProjectile(Projectile projectile)
    {
        _projectilesAlive.Remove(projectile);
        _projectilePool.Release(projectile);
    }

    public virtual void MoveProjectiles()
    {
        foreach (var projectile in _projectilesAlive.Where(projectile => projectile.enabled))
        {
            projectile.transform.position += (projectile.transform.position - transform.position).normalized * ((_weaponStat.Range / _weaponStat.BulletLifeTime) * Time.deltaTime);
        }
    }

    private bool TryShoot()
    {
        _colliders = Physics2D.OverlapCircleAll(transform.position, _weaponStat.Range)
            .Where(x => x.GetComponent<Enemy>() is not null).ToList();

        _target = GetNearestTarget(_colliders);

        return _target;
    }

    private Transform GetNearestTarget(List<Collider2D> targets)
    {
        float minDistance = _weaponStat.Range + 1;
        Transform target = null;
        foreach (Collider2D collider in targets) 
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

    protected virtual void Shoot()
    {
        SpawnProjectile();
    }

    protected void SpawnProjectile()
    {
        Projectile projectile = _projectilePool.Get();

        float angle = Random.Range(-_weaponStat.Spread/2, _weaponStat.Spread/2) * Mathf.Deg2Rad;

        Vector3 direction = (_target.position - transform.position).normalized;
        
        direction = new Vector3(
            direction.x * Mathf.Cos(angle) - direction.y * Mathf.Sin(angle), 
            direction.x * Mathf.Sin(angle) + direction.y * Mathf.Cos(angle), 
            0
            );
        
        projectile.transform.position = transform.position + direction/10;
        projectile.DespawnCooldown = _weaponStat.BulletLifeTime; 
        projectile.Pierce = _weaponStat.Pierce;
        projectile.transform.localScale = Vector3.one * _weaponStat.BulletSize;
        _projectilesAlive.Add(projectile);
        projectile.OnDestroy.RemoveAllListeners();
        projectile.OnDestroy.AddListener(RemoveProjectile);
    }
    
    public void OnDrawGizmos()
    {
        if (!_weaponStat) return;
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _weaponStat.Range);
    }

    public void Upgrade()
    {
        _weaponStat = _upgradeStats[Level];
        
        Level++;
        
        _cooldown = _weaponStat.ReloadTime;
    }

    public bool CanUpgrade(int Money)
    {
        return Level < _upgradeStats.Count && Money >= _upgradeStats[Level].Price;
    }
}

public enum WeaponType
{
    Riffle,
    Sniper,
    Cannon
}