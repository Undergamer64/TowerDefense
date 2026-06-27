using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    [FormerlySerializedAs("Type")] public WeaponType _Type;

    [FormerlySerializedAs("_upgradeStats")] [SerializeField] public List<WeaponStat> _UpgradeStats;

    [SerializeField] protected GameObject _projectilePrefab;
 
    protected WeaponStat _weaponStat;
    private float _cooldown = 0;

    protected Transform _target;

    [SerializeField] protected int _poolCapacity = 50;
    [SerializeField] protected int _poolPreloadQuantity = 5;
    
    protected ComponentPool<Projectile> _projectilePool;
    protected List<Projectile> _projectilesAlive = new List<Projectile>();
    
    private List<Collider2D> _colliders = new List<Collider2D>();
    
    [FormerlySerializedAs("_level")] public int _Level;
    
    [SerializeField] private Color _gizmoColor = Color.green;
    
    private void Awake()
    {
        _projectilePool = new ComponentPool<Projectile>(_projectilePrefab, _poolCapacity, _poolPreloadQuantity, transform);
        if (_Level > 0)
        {
            _weaponStat = _UpgradeStats[_Level];
            _cooldown = _weaponStat.ReloadTime;
        }
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
        return _UpgradeStats[_Level].Price;
    }

    public int GetMaxUpgradeLevel()
    {
        return _UpgradeStats.Count;
    }
    
    protected void RemoveProjectile(Projectile projectile)
    {
        while (_projectilesAlive.Contains(projectile))
        {
            _projectilesAlive.Remove(projectile);
        }
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

        _target = null;
        
        switch (_Type)
        {
            case WeaponType.Sniper:
                _target = GetBiggestTarget(_colliders);
                break;
            default:
                _target = GetNearestTarget(_colliders);
                break;
        }

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

    public Transform GetBiggestTarget(List<Collider2D> targets)
    {
        bool typeBig = false;
        float maxHP = -1;
        List<Collider2D> colliders = new List<Collider2D>();
        foreach (Collider2D collider in targets)
        {
            if (!collider.TryGetComponent(out Enemy enemy)) continue;

            if (Mathf.Approximately(enemy._Life, maxHP) || (typeBig && enemy._Type == EnemyType.big))
            {
                colliders.Add(collider);
            }
            else if (enemy._Life > maxHP)
            {
                if (enemy._Type == EnemyType.big) typeBig = true;
                colliders.Clear();
                colliders.Add(collider);
                maxHP = enemy._Life;
            }
        }
        return GetNearestTarget(colliders);
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
        projectile.GetComponent<SpriteRenderer>().sprite = _weaponStat.BulletSprite;
        projectile.transform.localScale = Vector3.one * _weaponStat.BulletSize;
        if (projectile is CannonProjectile cannonProjectile)
        {
            cannonProjectile._explosionRange = _weaponStat.ExplosionRange;
            cannonProjectile.UpdateRadiusDisplay();
        }
        _projectilesAlive.Add(projectile);
        projectile.OnDestroy.RemoveAllListeners();
        projectile.OnDestroy.AddListener(RemoveProjectile);
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        foreach (WeaponStat stat in _UpgradeStats)
        {
            Gizmos.DrawWireSphere(transform.position, stat.Range);
        }
    }

    public void Upgrade()
    {
        _weaponStat = _UpgradeStats[_Level];
        
        _Level++;
        
        _cooldown = _weaponStat.ReloadTime;
    }

    public bool CanUpgrade(int Money)
    {
        return _Level < _UpgradeStats.Count && Money >= _UpgradeStats[_Level].Price;
    }
}

public enum WeaponType
{
    Riffle,
    Sniper,
    Cannon
}