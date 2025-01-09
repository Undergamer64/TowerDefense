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
    
    [SerializeField] private List<Sprite> _healthUpgradesSprites = new List<Sprite>();
    
    [SerializeField] private Button _healButton;
    
    [SerializeField] private List<Button> _buttons;
    
    public void OpenShop()
    {
        _shopPanel.SetActive(true);
        Time.timeScale = 0;

        List<Weapon> UpgradableWeapons = _tower.Weapons.Where(x => x._Level < x.GetMaxUpgradeLevel()).ToList();

        for (int i = 0; i < _buttons.Count; i++)
        {
            Button Button = _buttons[i];
            Button.gameObject.SetActive(true);
            Button.onClick.RemoveAllListeners();
            Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            if (UpgradableWeapons.Count < 1)
            {
                Button.gameObject.SetActive(false);
                continue;
            }
            Weapon weapon = UpgradableWeapons[Random.Range(0, UpgradableWeapons.Count)];
            Button.image.sprite = weapon._UpgradeStats[weapon._Level].CardSprite;
            Button.image.SetNativeSize();
            Button.onClick.AddListener(() =>
            {
                if (!_tower.TryUpgradeWeapon(weapon._Type)) return;
                Button.gameObject.SetActive(false);
            });
            Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Price :" + weapon.GetPrice();
            UpgradableWeapons.Remove(weapon);
        }
        
        _healButton.gameObject.SetActive(false);
        if (_healthUpgradesSprites.Count == 0 || _healButton == null) return;
        _healButton.gameObject.SetActive(true);
        _healButton.onClick.RemoveAllListeners();
        int HealIndex = Random.Range(0, _healthUpgradesSprites.Count);
        _healButton.image.sprite = _healthUpgradesSprites[HealIndex];
        _healButton.image.SetNativeSize();
        _healButton.onClick.AddListener(() =>
        {
            if (!_tower.TryHeal(HealIndex)) return;
            _healButton.gameObject.SetActive(false);
        });
        _healButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Price :" + (HealIndex + 2) * 5;
    }

    public void CloseShop()
    {
        Time.timeScale = 1;
        _shopPanel.SetActive(false);
    }
}