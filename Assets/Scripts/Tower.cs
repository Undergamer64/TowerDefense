using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Tower : MonoBehaviour
{
    public List<Weapon> Weapons = new List<Weapon>();

    public UnityEvent<Weapon> OnWeaponUpgrade = new UnityEvent<Weapon>();
    
    [SerializeField] private PlayerData _playerData;

    private void Start()
    {
        foreach (Weapon weapon in Weapons.Where(weapon => weapon._Level == 0))
        {
            weapon.gameObject.SetActive(false);
        }
    }

    public bool TryHeal(int healthIndex)
    {
        if ((healthIndex + 2) * 5 > _playerData._Money) return false;
        _playerData._Money -= (healthIndex + 2) * 5;
        Heal((healthIndex + 2) * 5);
        return true;
    }
    
    public void Heal(int amount)
    {
        _playerData._Life += amount;
    }

    void Update()
    {
        foreach (Weapon weapon in Weapons)
        {
            if (!weapon.gameObject.activeSelf || weapon._Level == 0) continue;
            weapon.Tick();
            weapon.MoveProjectiles();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Enemy enemy)) return;

        switch (enemy._Type)
        {
            case EnemyType.normal:
                _playerData._Life -= 2;
                break;
            case EnemyType.big:
                _playerData._Life -= 3;
                break;
            case EnemyType.groupe:
                _playerData._Life -= 1;
                break;
            default:
                break;
        }
        
        enemy._Life = 0;
        enemy.TakeDamage(1);
        if (_playerData._Life > 0) return;
        _playerData._Life = 0;
        
        //END GAME HERE
    }

    public bool TryUpgradeWeapon(WeaponType type)
    {
        List<Weapon> WeaponsToUpgrade = Weapons.FindAll(x => x._Type == type);
        
        bool canUpgrade = false;
        Weapon WeaponToUpgrade = null;
        foreach (Weapon weapon in WeaponsToUpgrade)
        {
            if (weapon.CanUpgrade(_playerData._Money))
            {
                WeaponToUpgrade = weapon;
                canUpgrade = true;
                break;
            }
        }
        
        if (canUpgrade)
        {
            _playerData._Money -= WeaponToUpgrade.GetPrice();
            WeaponToUpgrade.Upgrade();
            OnWeaponUpgrade.Invoke(WeaponToUpgrade);
        }
        
        return canUpgrade;
    }
}
