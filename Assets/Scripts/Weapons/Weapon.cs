using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private WeaponStat _weaponStatBase;

    [SerializeField]
    protected GameObject _projectilePrefab;
 
    protected WeaponStat _weaponStat;
    private float _cooldown;

    protected Transform _target;

    protected ComponentPool<Projectile> _projectilePool;
    public List<Projectile> ProjectilesAlive;


    private void Start()
    {
        _projectilePool = new ComponentPool<Projectile>(_projectilePrefab, 50, 10);
        _weaponStat = Instantiate(_weaponStatBase);
        _cooldown = _weaponStat.ReloadTime;
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

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _weaponStatBase.Range);
    }
}
