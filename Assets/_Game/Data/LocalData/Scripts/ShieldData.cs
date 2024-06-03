using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="ShieldData", menuName ="LocalData/ShieldData", order =4)]
public class ShieldData : ScriptableObject
{
    public List<ShieldItem> shieldList;
}


[System.Serializable]
public class ShieldItem
{
    public ShieldType shieldType;
    public Shield shield;
    public Sprite shieldSprite;
    public int shieldPrice;
}
