using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject _shopPanel;
    
    [SerializeField] private Tower _tower;
    
    [SerializeField] private List<Button> _buttons;
    
    public void OpenShop()
    {
        _shopPanel.SetActive(true);
        Time.timeScale = 0;

        List<Weapon> UpgradableWeapons = _tower.Weapons.Where(x => x.Level < 5).ToList();
        
        if (UpgradableWeapons.Count < _buttons.Count) return; //   /!\ A CHANGER !!!

        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].onClick.RemoveAllListeners();
            int WeaponIndex = Random.Range(0, UpgradableWeapons.Count);
            _buttons[i].onClick.AddListener(() => { _tower.TryUpgradeWeapons(UpgradableWeapons[WeaponIndex].Type, 0);});
        }
    }

    public void CloseShop()
    {
        Time.timeScale = 1;
        _shopPanel.SetActive(false);
    }
}