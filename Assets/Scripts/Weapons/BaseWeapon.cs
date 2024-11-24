using UnityEngine;

public class BaseWeapon : Weapon
{
    protected override void Shoot()
    {
        Projectile projectile = _projectilePool.Get();
        projectile.DespawnCooldown = _weaponStat.BulletLifeTime;
        projectile.transform.position = transform.position + (_target.position - transform.position).normalized/10;
        projectile.Spawner = this;
        ProjectilesAlive.Add(projectile);
    }

    public override void MoveProjectiles()
    {
        foreach (Projectile projectile in ProjectilesAlive)
        {
            if (!projectile.enabled)
            {
                Debug.Log("Projectile not removed from list");
                continue;
            }
            projectile.transform.position += (projectile.transform.position - transform.position).normalized * (_weaponStat.BulletLifeTime / _weaponStat.Range);
        }
    }
}
