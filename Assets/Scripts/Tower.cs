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
        enemy.Life = 0;
        enemy.TakeDamage(1);
        _playerData.Life -= 1;
        if (_playerData.Life > 0) return;
        _playerData.Life = 0;
        
        //END GAME HERE
    }

    public void TryUpgradeWeapon(WeaponType type)
    {
        Debug.Log(type.ToString());
        
        List<Weapon> WeaponsToUpgrade = Weapons.FindAll(x => x.Type == type);
        
        bool success = false;
        Weapon WeaponToUpgrade = null;
        foreach (Weapon weapon in WeaponsToUpgrade)
        {
            if (weapon.CanUpgrade(_playerData.Money))
            {
                WeaponToUpgrade = weapon;
                success = true;
                break;
            }
        }
        
        Debug.Log(success);
        
        if (success)
        {
            _playerData.Money -= WeaponToUpgrade.GetPrice();
            WeaponToUpgrade.Upgrade();
            OnWeaponUpgrade.Invoke(WeaponToUpgrade);
        }
    }
}
