using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStat", menuName = "ScriptableObjects/WeaponStat", order = 1)]
public class WeaponStat : ScriptableObject
{
    public float BulletDamage = 1f;
    public float Range = 3f;
    public float ReloadTime = 1f;
    public float BulletLifeTime = 1f;
    public int Pierce = 1;
    public float BulletSize = 1f;
    public float Spread = 0f;

    public int Price = 5;
}