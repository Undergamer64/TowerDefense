using System.Linq;
using UnityEngine;

public class CannonWeapon : Weapon
{
    protected override void Shoot()
    {
        Projectile projectile = _projectilePool.Get();
        projectile.DespawnCooldown = _weaponStat.BulletLifeTime;
        projectile.transform.position = transform.position + (_target.position - transform.position).normalized/10;
        projectile.Pierce = _projectilePrefab.GetComponent<Projectile>().Pierce;
        projectile.Spawner = this;
        ProjectilesAlive.Add(projectile);
    }

    public override void MoveProjectiles()
    {
        foreach (var projectile in ProjectilesAlive.Where(projectile => projectile.enabled))
        {
            projectile.transform.position += (projectile.transform.position - transform.position).normalized * ((_weaponStat.Range / _weaponStat.BulletLifeTime) * Time.deltaTime);
        }
    }
}
