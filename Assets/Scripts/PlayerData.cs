using TMPro;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int MaxLife = 5;
    public int Life = 5;
    public int Money;

    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _lifeText;
    
    public void Update()
    {
        _moneyText.text = "Money : " + Money.ToString();
        _lifeText.text = "Life : " + Life.ToString() + "/" + MaxLife.ToString();
    }
    
}
