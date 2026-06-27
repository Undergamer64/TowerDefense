using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerData : MonoBehaviour
{
    [FormerlySerializedAs("Life")] public int _Life = 5;
    [FormerlySerializedAs("Money")] public int _Money;

    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _lifeText;
    
    public void Update()
    {
        _moneyText.text = _Money.ToString();
        _lifeText.text = _Life.ToString();
    }
    
}
