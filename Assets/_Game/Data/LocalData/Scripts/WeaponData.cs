using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponData", menuName = "LocalData/WeaponData", order = 0)]
public class WeaponData : ScriptableObject
{
    [SerializeField] List<WeaponItem> weaponList;
    
    public Weapon GetWeapon(WeaponType wpType)
    {
        return weaponList[(int)wpType].weapon;
    }
}

[System.Serializable]
public class WeaponItem
{
    public WeaponType weaponType;
    public Weapon weapon;
}
