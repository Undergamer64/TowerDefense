using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tower : MonoBehaviour
{
    public List<Weapon> Weapons = new List<Weapon>();

    [SerializeField] private PlayerData _playerData;
    
    void Update()
    {
        foreach (Weapon weapon in Weapons)
        {
            if (weapon.Level == 0) continue;
            weapon.Tick();
            weapon.MoveProjectiles();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Enemy enemy)) return;
        enemy.Life = 0;
        enemy.TakeDamage(1);
        _playerData.Life -= 1;
        if (_playerData.Life > 0) return;
    }

    public void TryUpgradeWeapons(WeaponType type, int Price = 0)
    {
        if (_playerData.Money < Price)
        {
            return;
        }
        
        List<Weapon> WeaponToUpgrade = Weapons.FindAll(x => x.Type == type);
        
        bool success = false;
        foreach (Weapon weapon in WeaponToUpgrade)
        {
            if (weapon.TryUpgrade())
            {
                success = true;
            }
        }

        if (success)
        {
            _playerData.Money -= Price;
        }
    }
}
