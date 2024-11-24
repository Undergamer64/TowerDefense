using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private List<Weapon> _weapons = new List<Weapon>();

    void Update()
    {
        foreach (Weapon weapon in _weapons)
        {
            weapon.Tick();
            weapon.MoveProjectiles();
        }
    }
}
