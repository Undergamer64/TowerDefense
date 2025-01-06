using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private List<Button> _cards = new List<Button>();

    private List<Weapon> _currentWeaponsDisplayed = new();

    private int _weaponsEnabled = 0;
    
    [SerializeField] private Tower _tower;

    private void Start()
    {
        if (_tower is null)
        {
            return;
        }
        
        _tower.OnWeaponUpgrade.AddListener(WeaponChangeSprite);
        
        foreach (Weapon weapon in _tower.Weapons.Where(weapon => weapon._Level > 0))
        {
            _cards[_currentWeaponsDisplayed.Count].gameObject.SetActive(true);
                
            _cards[_currentWeaponsDisplayed.Count].image.sprite = weapon._UpgradeStats[weapon._Level-1].CardSprite;
                
            _currentWeaponsDisplayed.Add(weapon);
        }
    }

    private void WeaponChangeSprite(Weapon weaponUgraded)
    {
        if (_currentWeaponsDisplayed.Contains(weaponUgraded))
        {
            _cards[_currentWeaponsDisplayed.IndexOf(weaponUgraded)].image.sprite = weaponUgraded._UpgradeStats[weaponUgraded._Level-1].CardSprite;
        }
        else
        {
            _cards[_currentWeaponsDisplayed.Count].gameObject.SetActive(true);
                
            _cards[_currentWeaponsDisplayed.IndexOf(weaponUgraded)].image.sprite = weaponUgraded._UpgradeStats[weaponUgraded._Level-1].CardSprite;
                
            _currentWeaponsDisplayed.Add(weaponUgraded);
           
        }
    }
    
    
}
