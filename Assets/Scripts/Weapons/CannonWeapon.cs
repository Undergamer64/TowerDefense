using System.Linq;
using UnityEngine;

public class CannonWeapon : Weapon
{
    protected override void Shoot()
    {
        SpawnProjectile();
    }

    public override void MoveProjectiles()
    {
        foreach (var projectile in _projectilesAlive.Where(projectile => projectile.enabled))
        {
            projectile.transform.position += (projectile.transform.position - transform.position).normalized * ((_weaponStat.Range / _weaponStat.BulletLifeTime) * Time.deltaTime);
        }
    }
}
