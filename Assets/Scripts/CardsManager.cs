using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private List<Button> _cards = new List<Button>();
    private List<bool> _cardsActive = new List<bool>();
    
    private List<Weapon> _currentWeaponsDisplayed = new();

    [SerializeField] private TextMeshProUGUI _weaponsDisplayedText;
    
    [SerializeField] private int _maxWeaponsEnabled = 3;
    private int _weaponsEnabled = 0;
    
    [SerializeField] private Tower _tower;

    private void Start()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            int cardIndex = i;
            
            _cards[cardIndex].gameObject.SetActive(false);
            _cards[cardIndex].onClick.RemoveAllListeners();
            
            _cards[cardIndex].onClick.AddListener(() => { ChangeCardState(cardIndex);});
            
            _cardsActive.Add(false);
        }
        
        if (_tower is null)
        {
            return;
        }
        
        _tower.OnWeaponUpgrade.AddListener(WeaponChangeSprite);
        
        foreach (Weapon weapon in _tower.Weapons.Where(weapon => weapon._Level > 0))
        {
            _cards[_currentWeaponsDisplayed.Count].gameObject.SetActive(true);
            _currentWeaponsDisplayed.Add(weapon);
            ChangeCardState(_currentWeaponsDisplayed.Count - 1, true);
            
            ChangeCardSprite(weapon);
        }
    }

    private void WeaponChangeSprite(Weapon weaponUpgraded)
    {
        if (_currentWeaponsDisplayed.Contains(weaponUpgraded))
        {
            _cards[_currentWeaponsDisplayed.IndexOf(weaponUpgraded)].image.sprite = weaponUpgraded._UpgradeStats[weaponUpgraded._Level-1].CardSprite;
        }
        else
        {
            _cards[_currentWeaponsDisplayed.Count].gameObject.SetActive(true);
            _currentWeaponsDisplayed.Add(weaponUpgraded);
            ChangeCardState(_currentWeaponsDisplayed.Count - 1, true);

            ChangeCardSprite(weaponUpgraded);
        }
    }

    private void ChangeCardSprite(Weapon weapon)
    {
        int weaponIndex = _currentWeaponsDisplayed.IndexOf(weapon);

        if (weaponIndex >= _cards.Count)
        {
            return;
        }
        
        _cards[weaponIndex].image.sprite = weapon._UpgradeStats[weapon._Level-1].CardSprite;
        
        _cards[weaponIndex].image.SetNativeSize();
    }

    /// <summary>
    /// Set state
    /// </summary>
    /// <param name="cardIndex"></param>
    private void ChangeCardState(int cardIndex, bool state)
    {
        if (!state)
        {
            _cardsActive[cardIndex] = false;
            _weaponsEnabled--;
        }
        else if (_weaponsEnabled < _maxWeaponsEnabled)
        {
            _cardsActive[cardIndex] = true;
            _weaponsEnabled++;
        }
        
        _currentWeaponsDisplayed[cardIndex].gameObject.SetActive(_cardsActive[cardIndex]);
        //_cards[cardIndex].gameObject.SetActive(_cardsActive[cardIndex]);
        
        UpdateUI(cardIndex);
    }
    
    /// <summary>
    /// Switchs state
    /// </summary>
    /// <param name="cardIndex"></param>
    private void ChangeCardState(int cardIndex)
    {
        ChangeCardState(cardIndex, !_cardsActive[cardIndex]);
    }

    private void UpdateUI(int cardIndex)
    {
        if (_weaponsDisplayedText)
        {
            _weaponsDisplayedText.text = _weaponsEnabled.ToString() + '/' + _maxWeaponsEnabled.ToString() + " Active Cards";
        }

        _cards[cardIndex].image.color = _cardsActive[cardIndex] ? Color.gray : Color.white;
    }
}
