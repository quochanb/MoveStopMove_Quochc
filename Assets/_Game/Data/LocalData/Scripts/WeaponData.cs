using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "WeaponData", menuName = "LocalData/WeaponData", order = 0)]
public class WeaponData : ScriptableObject
{
    public List<WeaponItem> weaponList;
    
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
    public Sprite sprite;
    public string name;
    public int price;
    public string wpProperty;
}
