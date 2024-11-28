using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

        Debug.Log(_tower.Weapons.Count);
        List<Weapon> UpgradableWeapons = _tower.Weapons.Where(x => x.Level < x.GetMaxUpgradeLevel()).ToList();
        
        Debug.Log(UpgradableWeapons.Count);
        Debug.Log(UpgradableWeapons);

        for (int i = 0; i < _buttons.Count; i++)
        {
            Button Button = _buttons[i];
            Button.gameObject.SetActive(true);
            Button.onClick.RemoveAllListeners();
            Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            if (i >= UpgradableWeapons.Count)
            {
                continue;
            }
            int WeaponIndex = Random.Range(0, UpgradableWeapons.Count);
            Button.onClick.AddListener(() => { _tower.TryUpgradeWeapon(UpgradableWeapons[WeaponIndex].Type);});
            Button.onClick.AddListener(() => { Button.gameObject.SetActive(false);});
            Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Price :" + UpgradableWeapons[WeaponIndex].GetPrice();
        }
    }

    public void CloseShop()
    {
        Time.timeScale = 1;
        _shopPanel.SetActive(false);
    }
}