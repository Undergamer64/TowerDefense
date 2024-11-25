using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private List<Weapon> _weapons = new List<Weapon>();

    [SerializeField]
    private PlayerData _playerData;
    
    void Update()
    {
        foreach (Weapon weapon in _weapons)
        {
            weapon.Tick();
            weapon.MoveProjectiles();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Enemy enemy)) return;
        enemy.life = 0;
        enemy.TakeDamage(1);
        _playerData.Life -= 1;
        if (_playerData.Life > 0) return;
    }
}
